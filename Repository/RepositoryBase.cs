using Ardalis.GuardClauses;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository
{
    /// <summary>
    /// The cosmos repository base class.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class, new()
    {
        private readonly CosmosClient cosmosClient;
        private readonly Lazy<Container> container;
        private readonly int resourceDocumentsRequestLimit;
        private readonly ILogger<RepositoryBase<TEntity>> logger;
        private readonly ICosmosFeedReader cosmosFeedReader;
        private readonly int partitionKeyLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryBase{TEntity}"/> class.
        /// </summary>
        /// <param name="cosmosClient">The cosmos client instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cosmosFeedReader">The linq query.</param>
        /// <param name="containerReference">The container reference as specified in appsettings.</param>
        protected RepositoryBase(
            CosmosClient cosmosClient,
            IConfiguration configuration,
            ILogger<RepositoryBase<TEntity>> logger,
            ICosmosFeedReader cosmosFeedReader,
            string containerReference)
        {
            this.cosmosClient = Guard.Against.Null(cosmosClient, nameof(cosmosClient));
            var config = Guard.Against.Null(configuration, nameof(configuration));
            this.cosmosFeedReader = Guard.Against.Null(cosmosFeedReader, nameof(cosmosFeedReader));
            this.logger = Guard.Against.Null(logger, nameof(logger));

            string dbConfigPrefix = "Repository:CosmosDb";

            string dbName = config[$"{dbConfigPrefix}:DbName"];
            string containerName = Guard.Against.NullOrEmpty(
                config[$"{dbConfigPrefix}:Containers:{containerReference}:ContainerName"],
                nameof(containerName));

            this.partitionKeyLength = Convert.ToInt32(config[$"{dbConfigPrefix}:Containers:{containerReference}:KeyLength"]);

            if (!int.TryParse(config[$"{dbConfigPrefix}:Containers:{containerReference}:ResourceDocumentsRequestLimit"], out this.resourceDocumentsRequestLimit))
            {
                this.resourceDocumentsRequestLimit = 1000;
            }

            this.container = new Lazy<Container>(() =>
            {
                return this.cosmosClient.GetContainer(dbName, containerName);
            });
        }

        /// <inheritdoc/>
        public async ValueTask<ObjectResult> AddAsync(TEntity entity, string partitionKey)
        {
            this.logger.LogDebug(nameof(this.AddAsync));
            Guard.Against.Null(entity, nameof(entity));

            PartitionKey pk = new(partitionKey);

            ItemResponse<TEntity> response = await this.container.Value.UpsertItemAsync<TEntity>(entity, pk).ConfigureAwait(false);
            return CreateSuccessResponse(response.Resource, response.StatusCode);
        }

        /// <inheritdoc/>
        public virtual async ValueTask<ObjectResult> GetAllAsync(
            int? skip = null,
            int? top = null)
        {
            this.logger.LogDebug(nameof(this.GetAllAsync));

            var query = this.container.Value.GetItemLinqQueryable<TEntity>();

            var pagedQuery = this.SkipTopPagination(query, skip, top);

            var items = await this.cosmosFeedReader.GetItems(pagedQuery, CosmosLinqExtensions.ToFeedIterator);

            return CreateSuccessResponse(items);
        }

        /// <inheritdoc/>
        public virtual async ValueTask<ObjectResult> GetAllAsync(
            Expression<Func<TEntity, bool>> existsPredicate,
            int? skip = null,
            int? top = null)
        {
            this.logger.LogDebug(nameof(this.GetAllAsync));

            var query = this.container.Value.GetItemLinqQueryable<TEntity>()
                .Where(existsPredicate);

            var pagedQuery = this.SkipTopPagination(query, skip, top);

            var items = await this.cosmosFeedReader.GetItems(pagedQuery, CosmosLinqExtensions.ToFeedIterator);

            return CreateSuccessResponse(items);
        }

        /// <inheritdoc/>
        public virtual async ValueTask<ObjectResult> GetAsync(string id)
        {
            this.logger.LogDebug(nameof(this.GetAsync));
            Guard.Against.Null(id, nameof(id));

            ItemResponse<TEntity> item = await this.container.Value.ReadItemAsync<TEntity>(
                id,
                new PartitionKey(id));

            return CreateSuccessResponse(item.Resource, item.StatusCode);

            // if item does not match specified predicate
            throw new CosmosException("Item not found", HttpStatusCode.NotFound, default, default, default);
        }

        public async ValueTask<ObjectResult> DeleteAsync(string id)
        {
            this.logger.LogDebug(nameof(this.GetAsync));
            Guard.Against.Null(id, nameof(id));

            ItemResponse<TEntity> item = await this.container.Value.DeleteItemAsync<TEntity>(id, new PartitionKey(id));

            return CreateSuccessResponse(item, item.StatusCode);
        }

        private static ObjectResult CreateSuccessResponse<T>(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ObjectResult(data)
            {
                StatusCode = (int)statusCode
            };
        }

        private IQueryable<TEntity> SkipTopPagination(IQueryable<TEntity> iq, int? skip, int? top)
        {
            Guard.Against.Null(iq, nameof(iq));

            if (skip.HasValue)
            {
                iq = iq.Skip(skip.Value);
            }

            int topValue = top.HasValue && top < this.resourceDocumentsRequestLimit ?
                top.Value :
                this.resourceDocumentsRequestLimit;

            return iq.Take(topValue);
        }
    }
}

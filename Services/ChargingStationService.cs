using Ardalis.GuardClauses;
using ChargingStationApi.Commands;
using ChargingStationApi.Models;
using ChargingStationApi.Repository;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace ChargingStationApi.Services
{

    public class ChargingStationService : IChargingStationService
    {
        private readonly IChargingStationCosmosRepository repository;
        private readonly IMemoryCache memoryCache;

        public ChargingStationService(
            IChargingStationCosmosRepository repository,
            ILogger<ChargingStationService> logger,
            IMemoryCache memoryCache)
        {
            this.repository = Guard.Against.Null(repository, nameof(repository));
            this.memoryCache = memoryCache;
        }

        public async Task<ResponseTemplate<RepoChargingStationEntity>> CreateChargingStationAsync(ChargingStationModel postChargingStationModel)
        {
            // Cacheing can be implemented e.g. in Get Single
            // FluentValidation can be used here to validate the payload.
            Guard.Against.Null(postChargingStationModel, nameof(postChargingStationModel));

            // Automapper can be used here
            RepoChargingStationEntity repoChargingStationEntity = new()
            {
                Id = Guid.NewGuid(),
                csId = postChargingStationModel.csId,
                Comment = postChargingStationModel.Comment,
                GroupId = postChargingStationModel.GroupId,
                Latitude = postChargingStationModel.Latitude,
                Longitude = postChargingStationModel.Longitude,
                Name = postChargingStationModel.Name,
                OwnerId = postChargingStationModel.OwnerId,
                ProtocolVersion = postChargingStationModel.ProtocolVersion,
            };

            return await this.repository.AddAsync(repoChargingStationEntity, repoChargingStationEntity.ProtocolVersion.ToString());
        }

        public async Task<ResponseTemplate<RepoChargingStationEntity>> DeleteChargingStationAsync(string id)
        {
            return await this.repository.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<ResponseTemplate<RepoChargingStationEntity>> GetChargingStationAsync(string id)
        {
            ResponseTemplate<RepoChargingStationEntity> responseTemplate = new();
            RepoChargingStationEntity repoChargingStationEntity;

            // If found in cache, return cached data
            if (memoryCache.TryGetValue(GetCacheKey(id), out repoChargingStationEntity))
            {
                responseTemplate.Data = repoChargingStationEntity;
                responseTemplate.StatusCode = HttpStatusCode.OK;

                return responseTemplate;
            }

            responseTemplate = await this.repository.GetAsync(id).ConfigureAwait(false);

            if (responseTemplate.StatusCode == HttpStatusCode.OK)
            {
                this.UpdateCache(GetCacheKey(id), responseTemplate.Data);
                return responseTemplate;
            }
            else
            {
                return responseTemplate;
            }
        }

        public async Task<ResponseTemplate<List<RepoChargingStationEntity>>> GetChargingStationsAsync(GetChargingStationsCommand request)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(request.QueryCollection, nameof(request.QueryCollection));

            var top = 0; //request.QueryCollection["top"];
            var skip = 20; //request.QueryCollection["skip"];

            if (!string.IsNullOrEmpty(request.ProtocolVersion))
            {
                Expression<Func<RepoChargingStationEntity, bool>> where = (RepoChargingStationEntity item) => item.ProtocolVersion == request.ProtocolVersion;

                return await this.repository.GetAllAsync(0, 20, where);
            }

            return await this.repository.GetAllAsync(0, 20);
        }

        private string GetCacheKey(string csId)
        {
            return $"csid:{csId}";
        }

        private void UpdateCache(string key, RepoChargingStationEntity repoChargingStationEntity)
        {
            // Set cache options
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30));

            // Set object in cache
            memoryCache.Set(key, repoChargingStationEntity, cacheOptions);
        }
    }
}

using ChargingStationApi.Models;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository
{
    public class ChargingStationCosmosRepository : RepositoryBase<RepoChargingStationEntity>, IChargingStationCosmosRepository
    {
        public ChargingStationCosmosRepository(
            CosmosClient cosmosClient,
            IConfiguration configuration,
            ILogger<RepositoryBase<RepoChargingStationEntity>> logger,
            ICosmosFeedReader cosmosFeedReader)
            :base(
                 cosmosClient,
                 configuration,
                 logger,
                 cosmosFeedReader,
                 "ChargingStations")
        {}

        public virtual ValueTask<ObjectResult> AddAsync(ChargingStationModel entity, string partitionKey)
        {
            RepoChargingStationEntity repoChargingStationEntity = new()
            {
                Id = Guid.NewGuid(),
                csId = entity.csId,
                Comment = entity.Comment,
                GroupId = entity.GroupId,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,
                Name = entity.Name,
                OwnerId = entity.OwnerId,
                ProtocolVersion = entity.ProtocolVersion,
            };

            return  base.AddAsync(repoChargingStationEntity, repoChargingStationEntity.Id.ToString());
        }

        public virtual ValueTask<ObjectResult> GetAllAsync(int? skip = null, int? top = null, string protocolVersion = null)
        {
            if (!string.IsNullOrEmpty(protocolVersion))
            {
                Expression<Func<RepoChargingStationEntity, bool>> where = (RepoChargingStationEntity item) => item.ProtocolVersion == protocolVersion;
                return base.GetAllAsync(where, skip, top);
            }

            return base.GetAllAsync(skip, top);
        }

        // Method if more complex filter needed.
        public virtual ValueTask<ObjectResult> GetAllAsync(Expression<Func<ChargingStationModel, bool>> existsPredicate, int? skip = null, int? top = null)
        {
            throw new NotImplementedException();
        }

        public new ValueTask<ObjectResult> GetAsync(string id)
        {
            return base.GetAsync(id);
        }
    }
}

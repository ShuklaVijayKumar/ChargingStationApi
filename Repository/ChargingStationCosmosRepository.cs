using ChargingStationApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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

        public virtual ValueTask<ObjectResult> GetAllAsync<TKey>(int? skip = null, int? top = null)
        {
            return base.GetAllAsync(skip, top);
        }

        public virtual ValueTask<ObjectResult> GetAsync(string id)
        {
            return base.GetAsync(id);
        }
    }
}

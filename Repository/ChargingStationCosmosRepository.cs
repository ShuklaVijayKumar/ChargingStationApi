using ChargingStationApi.Models;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
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

        public new ValueTask<ResponseTemplate<RepoChargingStationEntity>> AddAsync(RepoChargingStationEntity entity, string partitionKey)
        {
            return base.AddAsync(entity, entity.Id.ToString());
        }

        public override ValueTask<ResponseTemplate<RepoChargingStationEntity>> DeleteAsync(string id)
        {
            return base.DeleteAsync(id);
        }

        public override ValueTask<ResponseTemplate<RepoChargingStationEntity>> GetAsync(string id)
        {
            return base.GetAsync(id);
        }

        public new ValueTask<ResponseTemplate<List<RepoChargingStationEntity>>> GetAllAsync(int? top, int? skip)
        {
            return base.GetAllAsync(top, skip);
        }

        public new ValueTask<ResponseTemplate<List<RepoChargingStationEntity>>> GetAllAsync(int? top, int? skip, Expression<Func<RepoChargingStationEntity, bool>> existsPredicate)
        {
            return base.GetAllAsync(top, skip, existsPredicate);
        }
    }
}

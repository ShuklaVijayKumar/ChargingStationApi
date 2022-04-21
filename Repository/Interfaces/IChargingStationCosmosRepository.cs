using ChargingStationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository.Interfaces
{
    public interface IChargingStationCosmosRepository
    {
        ValueTask<ResponseTemplate<RepoChargingStationEntity>> GetAsync(string id);

        ValueTask<ResponseTemplate<RepoChargingStationEntity>> DeleteAsync(string id);

        ValueTask<ResponseTemplate<RepoChargingStationEntity>> AddAsync(RepoChargingStationEntity entity, string partitionKey);

        ValueTask<ResponseTemplate<List<RepoChargingStationEntity>>> GetAllAsync(int? top, int? skip);

        ValueTask<ResponseTemplate<List<RepoChargingStationEntity>>> GetAllAsync(int? top, int? skip, Expression<Func<RepoChargingStationEntity, bool>> existsPredicate);
    }
}

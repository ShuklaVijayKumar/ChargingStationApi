using Ardalis.GuardClauses;
using ChargingStationApi.Commands;
using ChargingStationApi.Models;
using ChargingStationApi.Repository;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ChargingStationApi.Services
{

    public class ChargingStationService : IChargingStationService
    {
        private readonly IChargingStationCosmosRepository repository;

        public ChargingStationService(
            IChargingStationCosmosRepository repository,
            ILogger<ChargingStationService> logger)
        {
            this.repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<ResponseTemplate<RepoChargingStationEntity>> CreateChargingStationAsync(ChargingStationModel postChargingStationModel)
        {
            Guard.Against.Null(postChargingStationModel, nameof(postChargingStationModel));

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
            return await this.repository.DeleteAsync(id).ConfigureAwait(false);
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
    }
}

using Ardalis.GuardClauses;
using ChargingStationApi.Commands;
using ChargingStationApi.Models;
using ChargingStationApi.Repository;
using ChargingStationApi.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        
        public async Task<ObjectResult> CreateChargingStationAsync(ChargingStationModel postChargingStationModel)
        {
            Guard.Against.Null(postChargingStationModel, nameof(postChargingStationModel));

            return await this.repository.AddAsync(postChargingStationModel, postChargingStationModel.ProtocolVersion.ToString());
        }

        public async Task<ObjectResult> GetChargingStationsAsync(GetChargingStationsCommand request)
        {
            Guard.Against.Null(request, nameof(request));
            Guard.Against.Null(request.QueryCollection, nameof(request.QueryCollection));

            var top = request.QueryCollection["top"];
            var skip = request.QueryCollection["skip"];

            // passing the default value for the simplicity.
            return await this.repository.GetAllAsync(0, 20, request.ProtocolVersion);
        }

        public async Task<ObjectResult> GetChargingStationAsync(string id)
        {
            return await this.repository.GetAsync(id);
        }

        public async Task<ObjectResult> DeleteChargingStationAsync(string id)
        {
            return await this.repository.DeleteAsync(id);
        }
    }
}

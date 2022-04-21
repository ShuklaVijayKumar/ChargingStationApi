using ChargingStationApi.Commands;
using ChargingStationApi.Models;
using ChargingStationApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChargingStationApi.Services
{
    public interface IChargingStationService
    {
        Task<ResponseTemplate<RepoChargingStationEntity>> CreateChargingStationAsync(ChargingStationModel postChargingStationModel);

        Task<ResponseTemplate<List<RepoChargingStationEntity>>> GetChargingStationsAsync(GetChargingStationsCommand request);

        Task<ResponseTemplate<RepoChargingStationEntity>> GetChargingStationAsync(string id);

        Task<ResponseTemplate<RepoChargingStationEntity>> DeleteChargingStationAsync(string id);
    }
}

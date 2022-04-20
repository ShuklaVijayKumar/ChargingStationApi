using ChargingStationApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStationApi.Services
{
    public interface IChargingStationService
    {
        Task<ObjectResult> CreateChargingStationAsync(ChargingStationModel postChargingStationModel);

        Task<ObjectResult> GetChargingStationsAsync(IQueryCollection queryCollection);

        Task<ObjectResult> GetChargingStationAsync(string id);

        Task<ObjectResult> DeleteChargingStationAsync(string id);
    }
}

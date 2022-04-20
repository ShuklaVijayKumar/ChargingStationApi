using ChargingStationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository
{
    public interface IChargingStationCosmosRepository: IRepository<ChargingStationModel>
    {
        ValueTask<ObjectResult> GetAsync(string id);

        ValueTask<ObjectResult> DeleteAsync(string id);

        ValueTask<ObjectResult> GetAllAsync(int? skip = null, int? top = null);
    }
}

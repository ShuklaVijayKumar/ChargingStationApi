using ChargingStationApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStationApi.Repository.Interfaces
{
    public interface IChargingStationCosmosRepository: IRepository<ChargingStationModel>
    {
        new ValueTask<ObjectResult> GetAsync(string id);
        
        new ValueTask<ObjectResult> DeleteAsync(string id);

        ValueTask<ObjectResult> GetAllAsync(int? skip = null, int? top = null, string protocolVersion = null);
    }
}

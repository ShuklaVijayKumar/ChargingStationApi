using System.Threading.Tasks;

namespace ChargingStationApi.CosmosInitializers.Interfaces
{
    public interface IMustInitialize : IInitializer
    {
        /// <summary>
        /// Concrete instances must implement initialize.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task InitializeAsync();
    }
}

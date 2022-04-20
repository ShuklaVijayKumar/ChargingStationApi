using ChargingStationApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChargingStationApi.Commands
{
    public class CreateChargingStationCommand: IRequest<IActionResult>
    {
        [JsonProperty("post_charging_station_model")]
        public ChargingStationModel PostChargingStationModel { get; set; }
    }
}

using ChargingStationApi.Commands;
using ChargingStationApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChargingStationApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EVChargingController : AbstractRequestController
    {
        public EVChargingController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// Method to create chargning station.
        /// </summary>
        /// <param name="postChargingStationModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostAnalysis(
            [FromBody] ChargingStationModel postChargingStationModel) => await this.ProcessRequest(
                new CreateChargingStationCommand
                {
                    PostChargingStationModel = postChargingStationModel,
                });

        /// <summary>
        /// Method to get all charging stations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string protocolVersion = null) => await this.ProcessRequest(
                new GetChargingStationsCommand
                {
                    QueryCollection = this.Request.Query,
                    ProtocolVersion = protocolVersion,
                });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle([FromRoute] string id) => await this.ProcessRequest(
                new GetChargingStationCommand
                {
                    Id = id
                });

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id) => await this.ProcessRequest(
                new DeleteChargingStationCommand
                {
                    Id = id,
                });
    }
}

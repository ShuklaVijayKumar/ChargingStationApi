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
        private readonly ILogger<EVChargingController> _logger;

        public EVChargingController(IMediator mediator)
            : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> PostAnalysis(
            [FromBody] ChargingStationModel postChargingStationModel) => await this.ProcessRequest(
                new CreateChargingStationCommand
                {
                    PostChargingStationModel = postChargingStationModel,
                });

        [HttpGet]
        public async Task<IActionResult> Get() => await this.ProcessRequest(
                new GetChargingStationsCommand
                {
                    QueryCollection = this.Request.Query,
                });

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle([FromRoute] string id) => await this.ProcessRequest(
                new GetChargingStationCommand
                {
                    Id = id,
                });

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id) => await this.ProcessRequest(
                new DeleteChargingStationCommand
                {
                    Id = id,
                });
    }
}

using Ardalis.GuardClauses;
using ChargingStationApi.Commands;
using ChargingStationApi.Handlers;
using ChargingStationApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingStationApi.Handlers
{
    public class GetChargingStationCommandHandler : IRequestHandler<GetChargingStationCommand, IActionResult>
    {
        private readonly ILogger<CreateChargingStationCommandHandler> logger;
        private readonly IChargingStationService chargingStationService;

        public GetChargingStationCommandHandler(
            ILogger<CreateChargingStationCommandHandler> logger,
            IChargingStationService chargingStationService)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.chargingStationService = chargingStationService;
        }

        public async Task<IActionResult> Handle(GetChargingStationCommand request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug(nameof(this.Handle));

            var svcResp = await this.chargingStationService.GetChargingStationAsync(request.Id);

            return new ObjectResult(svcResp)
            {
                StatusCode = (int)svcResp.StatusCode
            };
        }
    }
}

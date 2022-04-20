using Ardalis.GuardClauses;
using ChargingStationApi.Commands;
using ChargingStationApi.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingStationApi.Handlers
{
    public class CreateChargingStationCommandHandler: IRequestHandler<CreateChargingStationCommand, IActionResult>
    {
        private readonly ILogger<CreateChargingStationCommandHandler> logger;
        private readonly IChargingStationService chargingStationService;

        public CreateChargingStationCommandHandler(
            ILogger<CreateChargingStationCommandHandler> logger,
            IChargingStationService chargingStationService)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.chargingStationService = chargingStationService;
        }

        public async Task<IActionResult> Handle(CreateChargingStationCommand request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug(nameof(this.Handle));
            Guard.Against.Null(request, nameof(request));

            var svcResp = await this.chargingStationService.CreateChargingStationAsync(request.PostChargingStationModel);

            return new ObjectResult(svcResp)
            {
                StatusCode = svcResp.StatusCode,
            };
        }
    }
}

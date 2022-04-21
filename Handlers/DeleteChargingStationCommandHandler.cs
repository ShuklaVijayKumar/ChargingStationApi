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
    public class DeleteChargingStationCommandHandler : IRequestHandler<DeleteChargingStationCommand, IActionResult>
    {
        private readonly ILogger<CreateChargingStationCommandHandler> logger;
        private readonly IChargingStationService chargingStationService;

        public DeleteChargingStationCommandHandler(
            ILogger<CreateChargingStationCommandHandler> logger,
            IChargingStationService chargingStationService)
        {
            this.logger = Guard.Against.Null(logger, nameof(logger));
            this.chargingStationService = chargingStationService;
        }

        public async Task<IActionResult> Handle(DeleteChargingStationCommand request, CancellationToken cancellationToken)
        {
            this.logger.LogDebug(nameof(this.Handle));

            var svcResp = await this.chargingStationService.DeleteChargingStationAsync(request.Id);

            return new ObjectResult(svcResp)
            {
                StatusCode = (int)svcResp.StatusCode
            };
        }
    }
}

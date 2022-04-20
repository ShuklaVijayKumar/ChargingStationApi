using Ardalis.GuardClauses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ChargingStationApi.Controllers
{
    public abstract class AbstractRequestController : ControllerBase
    {
        private readonly IMediator mediator;

        protected AbstractRequestController(IMediator mediator)
        {
            this.mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        protected async Task<IActionResult> ProcessRequest(IRequest<IActionResult> command)
        {
            return await this.mediator.Send(command).ConfigureAwait(false);
        }
    }
}

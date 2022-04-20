using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChargingStationApi.Commands
{
    public class GetChargingStationCommand : IRequest<IActionResult>
    {
        public string Id { get; set; }
    }
}

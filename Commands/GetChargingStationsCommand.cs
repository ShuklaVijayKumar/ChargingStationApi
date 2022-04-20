using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChargingStationApi.Commands
{
    public class GetChargingStationsCommand : IRequest<IActionResult>
    {
        /// <summary>
        /// Gets or sets the query string parameters collection.
        /// </summary>
        public IQueryCollection QueryCollection { get; set; }
    }
}

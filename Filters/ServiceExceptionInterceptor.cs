using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace ChargingStationApi.Filters
{
    public class ServiceExceptionInterceptor : ExceptionFilterAttribute, IAsyncExceptionFilter
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            //Business exception-More generics for external world
            var error = new ErrorDetails()
            {
                StatusCode = 500,
                Message = "Something went wrong! Internal Server Error."
            };
            //Logs your technical exception with stack trace below

            context.Result = new JsonResult(error);
            return Task.CompletedTask;
        }
    }
}

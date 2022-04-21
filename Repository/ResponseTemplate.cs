using System.Net;

namespace ChargingStationApi.Repository
{
    public class ResponseTemplate<TData>
    {
        /// <summary>
        /// Gets or sets return data.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets of sets Statuscode.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}

using Newtonsoft.Json;
using System;

namespace ChargingStationApi.Models
{
    public class RepoChargingStationEntity : ChargingStationModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        //[JsonProperty("_etag")]
        //public string Etag { get; set; }


        //[JsonProperty("partition_key")]
        //public string PartitionKey { get; set; }
    }
}

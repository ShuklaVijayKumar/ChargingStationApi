namespace ChargingStationApi.Models
{
    public class PaginationModel
    {
        public int Top{ get; set; }

        public int Skip { get; set; }

        public string OrderBy { get; set; }
    }
}

namespace RailwayReservationAPI.Models.Dto
{
    public class TrackCreateRequestDTO
    {
        public string DepartureStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime ReturnTime { get; set;}
        public int TrainId { get; set; }
    }
}

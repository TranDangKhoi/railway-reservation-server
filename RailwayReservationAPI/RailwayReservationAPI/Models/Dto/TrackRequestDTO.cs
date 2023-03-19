using System.ComponentModel.DataAnnotations;

namespace RailwayReservationAPI.Models.Dto
{
    public class TrackRequestDTO
    {
        public string DepartureStation { get; set; }
        public DateTime DepartureTime { get; set; }
        public string ArrivalStation { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

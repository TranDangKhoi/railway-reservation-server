using System.ComponentModel.DataAnnotations;

namespace RailwayReservationAPI.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DepartureStation { get; set; }
        [Required]
        public DateTime DepartureTime { get; set; }
        [Required]
        public string ArrivalStation { get; set; }
        [Required]
        public DateTime ArrivalTime { get; set; }
        public ICollection<TrainTrack> TrainTracks { get; set; }
        
    }
}

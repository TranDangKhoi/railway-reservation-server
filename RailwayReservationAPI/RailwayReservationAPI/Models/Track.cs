using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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
        [Required]
        public DateTime ReturnTime { get; set; }
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public Train Train { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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
        public int TrainId { get; set; }
        [ForeignKey("TrainId")]
        public Train Train { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class Carriage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string CarriageNo { get; set; }
        [Required]
        public int TotalSeats { get; set; }
        public int TrainId { get; set; }
        [ForeignKey("TrainId")]
        public Train Train { get; set; }
    }
}

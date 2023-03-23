using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class Carriage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarriageNo { get; set; }
        [Required]
        public int TotalSeats { get; set; }
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public ICollection<Seat> Seats { get; set; }
        [ForeignKey("CarriageTypeId")]
        public int CarriageTypeId { get; set; }
        public CarriageType CarriageType { get; set; }
        
    }
}

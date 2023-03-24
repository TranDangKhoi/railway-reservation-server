using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class CarriageSeat
    {
        public int Id { get; set; }
        [ForeignKey("SeatId")]
        public int SeatId { get; set; }
        public Seat Seat {get; set;}
        public int CarriageId { get; set; }
    }
}

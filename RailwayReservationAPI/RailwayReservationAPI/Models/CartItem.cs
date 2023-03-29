using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        [ForeignKey("SeatId")]
        public Seat Seat { get; set; } = new();
        public int ShoppingCartId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("SeatId")]
        public int SeatId { get; set; }
        public Seat Seat { get; set; } = new();
        public int ShoppingCartId { get; set; }
    }
}

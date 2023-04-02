using System.ComponentModel.DataAnnotations;

namespace RailwayReservationAPI.Models.Dto
{
    public class OrderDetailsCreateDTO
    {
        public int SeatId { get; set; }
        public double Price { get; set; }
    }
}

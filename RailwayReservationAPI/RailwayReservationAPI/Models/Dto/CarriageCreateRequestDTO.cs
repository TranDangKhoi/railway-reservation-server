using System.ComponentModel.DataAnnotations;

namespace RailwayReservationAPI.Models.Dto
{
    public class CarriageCreateRequestDTO
    {
        [Required]
        public int CarriageNo { get; set; }
        [Required]
        public int TotalSeats { get; set; }
    }
}

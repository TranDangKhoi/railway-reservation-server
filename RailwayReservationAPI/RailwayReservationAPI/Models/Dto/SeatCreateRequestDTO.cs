using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models.Dto
{
    public class SeatCreateRequestDTO
    {
        public double SeatPrice { get; set; }
        public int SeatStatus { get; set; }
        public int CarriageId { get; set; }
    }
}

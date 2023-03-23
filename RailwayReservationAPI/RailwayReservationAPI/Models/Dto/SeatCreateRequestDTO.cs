using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models.Dto
{
    public class SeatCreateRequestDTO
    {
        public double SeatPrice { get; set; }
        public int SeatNo { get; set; }
        public int SeatStatus { get; set; }
    }
}

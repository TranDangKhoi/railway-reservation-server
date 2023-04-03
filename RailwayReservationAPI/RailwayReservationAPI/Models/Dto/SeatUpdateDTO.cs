using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models.Dto
{
    public class SeatUpdateDTO
    {
        public int Id { get; set; }
        public int? SeatNo { get; set; }
        public double SeatPrice { get; set; }
        public int SeatStatus { get; set; }
        public int CarriageId { get; set; }
    }
}

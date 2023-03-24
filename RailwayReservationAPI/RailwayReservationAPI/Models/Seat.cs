using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class Seat
    {
        public int Id { get; set; }
        public double SeatPrice { get; set; }
        public int SeatNo { get; set; }
        public int SeatStatus { get; set; }
        public ICollection<Carriage> Carriages { get; set; }
    }
}

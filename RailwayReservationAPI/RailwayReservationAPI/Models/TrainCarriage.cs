using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class TrainCarriage
    {
        public int Id { get; set; }
        public int TrainId { get; set; }
        [ForeignKey("CarriageId")]
        public int CarriageId { get; set;}
        public Carriage Carriage { get; set; }
    }
}

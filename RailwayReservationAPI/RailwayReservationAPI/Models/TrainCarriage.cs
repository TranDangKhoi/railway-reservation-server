using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class TrainCarriage
    {
        public int Id { get; set; }
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public Train Train { get; set; }
        public int CarriageId { get; set;}
    }
}

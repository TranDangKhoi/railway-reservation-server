using System.ComponentModel.DataAnnotations.Schema;

namespace RailwayReservationAPI.Models
{
    public class TrainTrack
    {
        public int Id { get; set; }
        public int TrackId { get; set;  }
        public int TrainId { get; set; }
        [ForeignKey("TrainId")]
        public Train Train { get; set; }
        
    }
}

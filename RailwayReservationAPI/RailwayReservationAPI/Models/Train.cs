using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RailwayReservationAPI.Models
{
    public class Train
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<Carriage> Carriages { get; set; }
        [NotMapped]
        public int TotalCarriages { get; set; }
    }
}

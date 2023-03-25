using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RailwayReservationAPI.Models.Dto
{
    public class CarriageUpdateRequestDTO
    {
        public int CarriageTypeId { get; set; }      
        public int TrainId { get; set; }
    }
}

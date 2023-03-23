namespace RailwayReservationAPI.Models.Dto
{
    public class TrainCreateRequestDTO
    {
        public string Name { get; set; }
        public IEnumerable<CarriageCreateRequestDTO> CarriageCreateRequestDTOs { get; set; }
    }
}

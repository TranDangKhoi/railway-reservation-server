namespace RailwayReservationAPI.Models.Dto
{
    public class TrainCreateRequestDTO
    {
        public string Name { get; set; }
        public IEnumerable<CarriageCreateRequestDTO> CarriageCreateRequestDTOs { get; set; }
        public IEnumerable<CarriageTypeCreateRequestDTO> CarriageTypeCreateRequestDTOs { get; set; }
        public IEnumerable<SeatCreateRequestDTO> SeatCreateRequestDTOs { get; set; }
    }
}

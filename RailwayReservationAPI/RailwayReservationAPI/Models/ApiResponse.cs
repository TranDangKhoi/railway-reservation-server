using System.Net;

namespace RailwayReservationAPI.Models
{
    public class ApiResponse
    {

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}

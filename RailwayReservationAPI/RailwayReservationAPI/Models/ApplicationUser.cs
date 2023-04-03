using Microsoft.AspNetCore.Identity;

namespace RailwayReservationAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}

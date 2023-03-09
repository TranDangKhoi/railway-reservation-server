using Microsoft.AspNetCore.Identity;

namespace RailwayReservationAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}

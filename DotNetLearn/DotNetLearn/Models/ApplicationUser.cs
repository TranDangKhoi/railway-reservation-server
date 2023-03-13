using Microsoft.AspNetCore.Identity;

namespace DotNetLearn.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Fullname { get; set; }
    }
}

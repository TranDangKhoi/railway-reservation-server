using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RailwayReservationAPI.Models;

namespace RailwayReservationAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<TrainTrack> TrainTracks { get; set; }
        public DbSet<Track> Tracks { get; set; }
    }
}

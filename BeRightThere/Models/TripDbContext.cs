using Microsoft.EntityFrameworkCore;

namespace BeRightThere.Models
{
    public class TripDbContext : DbContext
    {
        public TripDbContext(DbContextOptions<TripDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trip> Trips { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using ProjectCouchPotatoV1.Models;
using System.Reflection.Emit;

namespace ProjectCouchPotatoV1.Models
{
    public class MovieDbContext : DbContext
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {

        }
        public DbSet<MovieData> MoviesList { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }

    }
}

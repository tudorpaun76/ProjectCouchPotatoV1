using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectCouchPotatoV1.Areas.Identity.Data;
using ProjectCouchPotatoV1.Models;

namespace ProjectCouchPotatoV1.Models
{
    public class MovieDbContext : IdentityDbContext<ApplicationUser>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public DbSet<MovieData> MoviesList { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<MovieAvoid> MovieToAvoid { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Watchlist>()
                .Property(r => r.UserId)
                .IsRequired();

            modelBuilder.Entity<Review>()
    .Ignore(r => r.Cast);

            modelBuilder.Entity<Review>()
    .Ignore(r => r.Trailers);
        }
    }
}

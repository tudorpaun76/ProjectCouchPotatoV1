using Microsoft.EntityFrameworkCore;
using ProjectCouchPotatoV1.Models;
using System.Reflection.Emit;

namespace ProjectCouchPotatoV1.Models
{
    public class MovieDbContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
 
        }
    }
}

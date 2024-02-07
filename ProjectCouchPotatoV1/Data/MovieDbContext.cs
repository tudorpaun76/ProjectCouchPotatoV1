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
        public DbSet<Movie> Reviews { get; set; }
        
    }
}

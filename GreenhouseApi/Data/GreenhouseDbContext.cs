using GreenhouseApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenhouseApi.Data
{
    public class GreenhouseDbContext : DbContext
    {
        public GreenhouseDbContext(DbContextOptions<GreenhouseDbContext> options) 
            : base(options) { }

        public DbSet<Plant> Plants { get; set; }
    }
}
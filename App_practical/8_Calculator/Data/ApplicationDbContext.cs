using calculator.Models;
using Microsoft.EntityFrameworkCore;

namespace calculator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<CalculationHistory> CalculationHistory { get; set; }
    }
}

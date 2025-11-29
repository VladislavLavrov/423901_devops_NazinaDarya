using calculator.Data;
using Microsoft.EntityFrameworkCore;

namespace calculator.Data
{
    public class CalculatorContext : DbContext
    {
        public DbSet<DataInputVariant> DataInputVariants { get; set; }

        public CalculatorContext(DbContextOptions<CalculatorContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Базовая конфигурация
            base.OnModelCreating(modelBuilder);
        }
    }
}

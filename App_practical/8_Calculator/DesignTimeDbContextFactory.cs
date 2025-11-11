using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using calculator.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Используйте тот же ConnectionString что и в appsettings.json
        optionsBuilder.UseMySql(
            "Server=93.88.178.186;Port=5058;Database=8_WebCalculatorDb;Uid=root;Pwd=password;",
            ServerVersion.AutoDetect("Server=93.88.178.186;Port=5058;Database=8_WebCalculatorDb;Uid=root;Pwd=password;")
        );

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
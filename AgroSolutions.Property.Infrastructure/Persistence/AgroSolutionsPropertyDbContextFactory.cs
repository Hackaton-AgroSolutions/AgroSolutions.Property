using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AgroSolutions.Property.Infrastructure.Persistence;

public class AgroSolutionsPropertyDbContextFactory : IDesignTimeDbContextFactory<AgroSolutionsPropertyDbContext>
{
    public AgroSolutionsPropertyDbContext CreateDbContext(string[] args)
    {
        string basePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "..",
            "AgroSolutions.Property.API"
        );

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        DbContextOptions<AgroSolutionsPropertyDbContext> options = new DbContextOptionsBuilder<AgroSolutionsPropertyDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AgroSolutionsPropertyDbContext(options);
    }
}

using AgroSolutions.Property.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AgroSolutions.Property.Infrastructure.Persistence;

public class AgroSolutionsPropertyDbContext(DbContextOptions<AgroSolutionsPropertyDbContext> options) : DbContext(options)
{
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<Domain.Entities.Property> Properties { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}

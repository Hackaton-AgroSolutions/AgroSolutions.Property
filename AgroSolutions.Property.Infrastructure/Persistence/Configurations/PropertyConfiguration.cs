using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSolutions.Property.Infrastructure.Persistence.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Domain.Entities.Property>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Property> builder)
    {
        builder.HasKey(p => p.PropertyId);

        builder.HasIndex(p => new { p.PropertyId, p.Name }).IsUnique();
        builder.Property(p => p.UserId).IsRequired(true);
        builder.Property(p => p.Name).HasMaxLength(60);
        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.HasMany(p => p.Fields)
               .WithOne(f => f.Property)
               .HasForeignKey(f => f.PropertyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

using AgroSolutions.Property.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSolutions.Property.Infrastructure.Persistence.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.HasKey(f => f.FieldId);

        builder.HasIndex(f => new { f.PropertyId, f.Name }).IsUnique();
        builder.Property(f => f.Name).HasMaxLength(100);
        builder.Property(f => f.TotalAreaInHectares).HasPrecision(18, 2);

        builder.HasOne(f => f.Property)
               .WithMany(p => p.Fields)
               .HasForeignKey(f => f.PropertyId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

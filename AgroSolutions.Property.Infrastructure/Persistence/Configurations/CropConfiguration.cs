using AgroSolutions.Property.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AgroSolutions.Property.Infrastructure.Persistence.Configurations;

public class CropConfiguration : IEntityTypeConfiguration<Crop>
{
    public void Configure(EntityTypeBuilder<Crop> builder)
    {
        builder.HasKey(c => c.CropId);

        builder.HasIndex(c => c.Name).IsUnique();
        builder.Property(c => c.Name).HasMaxLength(20);

        builder.HasData([
            new(1, "Soja"),
            new(2, "Milho"),
            new(3, "Feijão"),
            new(4, "Arroz"),
            new(5, "Trigo"),
            new(6, "Algodão"),
            new(7, "Cana-de-açúcar"),
            new(8, "Café"),
            new(9, "Laranja"),
            new(10, "Tomate"),
            new(11, "Alface"),
            new(12, "Girassol"),
            new(13, "Aveia"),
            new(14, "Cevada"),
            new(15, "Batata"),
            new(16, "Cebola"),
            new(17, "Uva"),
            new(18, "Pinho"),
            new(19, "Capim"),
            new(20, "Eucalipto")
        ]);
    }
}

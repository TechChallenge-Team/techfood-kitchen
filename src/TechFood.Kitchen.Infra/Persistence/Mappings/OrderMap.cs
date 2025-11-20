using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TechFood.Kitchen.Infra.Persistence.Mappings;

public class OrderMap : IEntityTypeConfiguration<Kitchen.Domain.Entities.Order>
{
    public void Configure(EntityTypeBuilder<Kitchen.Domain.Entities.Order> builder)
    {
        builder.ToTable("Order");

        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .IsRequired();
    }
}

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TechFood.Kitchen.Domain.Entities;
using TechFood.Shared.Domain.Enums;
using TechFood.Shared.Infra.Persistence.Contexts;

namespace TechFood.Kitchen.Infra.Persistence.Contexts;

public class KitchenContext : TechFoodContext
{
    public DbSet<Preparation> Preparations { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    public KitchenContext(DbContextOptions<KitchenContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(KitchenContext).Assembly);

        var properties = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties());

        var stringProperties = properties.Where(p => p.ClrType == typeof(string));
        foreach (var property in stringProperties)
        {
            var maxLength = property.GetMaxLength() ?? 50;

            property.SetColumnType($"varchar({maxLength})");
        }

        var booleanProperties = properties
            .Where(p => p.ClrType == typeof(bool) ||
                        p.ClrType == typeof(bool?));

        foreach (var property in booleanProperties)
        {
            property.SetColumnType("bit");
            property.IsNullable = false;
        }

        var dateTimeProperties = properties.Where(p => p.ClrType == typeof(DateTime));

        foreach (var property in dateTimeProperties)
        {
            property.SetColumnType("datetime");
        }

        var enumProperties = properties.Where(p => p.ClrType == typeof(Enum));

        foreach (var property in enumProperties)
        {
            property.SetColumnType("smallint");
        }

        var amountProperties = properties
            .Where(p => p.ClrType == typeof(decimal) ||
                        p.ClrType == typeof(decimal?));

        foreach (var property in amountProperties)
        {
            property.SetColumnType("decimal(6, 2)");
        }

        SeedContext(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private static void SeedContext(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Preparation>()
            .HasData(
                new
                {
                    Id = new Guid("9b50f871-b829-4085-8ae5-118cd1198fbe"),
                    Number = 1,
                    OrderId = new Guid("d1b5f3a2-4c8e-4b7c-9f0e-5a2d6f3b8c1e"),
                    CreatedAt = new DateTime(2025, 5, 13, 22, 2, 36, DateTimeKind.Utc)
                        .AddTicks(6053),
                    Status = PreparationStatusType.Pending,
                    IsDeleted = false
                },
                new
                {
                    Id = new Guid("83874d8f-0bc8-42ab-85d9-540a36dcccf4"),
                    Number = 2,
                    OrderId = new Guid("f2b5f3a2-4c8e-4b7c-9f0e-5a2d6f3b8c1e"),
                    CreatedAt = new DateTime(2025, 5, 13, 22, 2, 36, DateTimeKind.Utc)
                        .AddTicks(6354),
                    Status = PreparationStatusType.Ready,
                    IsDeleted = false
                }
            );

        //modelBuilder.Entity<PaymentType>()
        //    .HasData(
        //        new { Id = 1, Code = "MCMA", Description = "Mastercard" },
        //        new { Id = 2, Code = "VIS", Description = "Visa" },
        //        new { Id = 3, Code = "ELO", Description = "Elo" },
        //        new { Id = 4, Code = "DNR", Description = "Sodexo" },
        //        new { Id = 5, Code = "VR", Description = "Vale Refeição" },
        //        new { Id = 6, Code = "PIX", Description = "Pix" }
        //    );
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        optionsBuilder.LogTo(Console.WriteLine);
#endif
    }
}
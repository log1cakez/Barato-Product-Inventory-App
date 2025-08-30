using BaratoInventory.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BaratoInventory.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed data
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Laptop",
                Category = "Electronics",
                Price = 999.99m,
                Quantity = 10,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 2,
                Name = "Mouse",
                Category = "Electronics",
                Price = 29.99m,
                Quantity = 50,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 3,
                Name = "Desk Chair",
                Category = "Furniture",
                Price = 199.99m,
                Quantity = 15,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Id = 4,
                Name = "Coffee Mug",
                Category = "Kitchen",
                Price = 12.99m,
                Quantity = 100,
                CreatedAt = DateTime.UtcNow
            }
        );
    }
}

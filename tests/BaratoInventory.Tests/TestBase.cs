using Microsoft.EntityFrameworkCore;
using BaratoInventory.Infrastructure.Data;
using NUnit.Framework;

namespace BaratoInventory.Tests;

public abstract class TestBase
{
    protected ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    protected async Task<ApplicationDbContext> CreateContextWithDataAsync(IEnumerable<object> entities)
    {
        var context = CreateContext();
        
        foreach (var entity in entities)
        {
            context.Add(entity);
        }
        
        await context.SaveChangesAsync();
        return context;
    }

    protected async Task CleanupContextAsync(ApplicationDbContext context)
    {
        if (context != null)
        {
            context.Database.EnsureDeleted();
            await context.DisposeAsync();
        }
    }

    protected static void AssertProductProperties(Product product, string expectedName, string expectedCategory, decimal expectedPrice, int expectedQuantity)
    {
        Assert.Multiple(() =>
        {
            Assert.That(product.Name, Is.EqualTo(expectedName));
            Assert.That(product.Category, Is.EqualTo(expectedCategory));
            Assert.That(product.Price, Is.EqualTo(expectedPrice));
            Assert.That(product.Quantity, Is.EqualTo(expectedQuantity));
        });
    }

    protected static void AssertProductExists(Product product)
    {
        Assert.Multiple(() =>
        {
            Assert.That(product, Is.Not.Null);
            Assert.That(product.Id, Is.GreaterThan(0));
            Assert.That(product.CreatedAt, Is.GreaterThan(DateTime.UtcNow.AddSeconds(-1)));
        });
    }

    protected static void AssertExceptionMessage(Exception ex, string expectedMessage)
    {
        Assert.That(ex.Message, Does.Contain(expectedMessage));
    }
}

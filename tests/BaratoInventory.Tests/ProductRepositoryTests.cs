using BaratoInventory.Core.Entities;
using BaratoInventory.Infrastructure.Data;
using BaratoInventory.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace BaratoInventory.Tests;

[TestFixture]
public class ProductRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new ProductRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllAsync_WithProducts_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new Product { Name = "Product 1", Category = "Electronics", Price = 100m, Quantity = 10 },
            new Product { Name = "Product 2", Category = "Furniture", Price = 200m, Quantity = 5 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Any(p => p.Name == "Product 1"), Is.True);
        Assert.That(result.Any(p => p.Name == "Product 2"), Is.True);
    }

    [Test]
    public async Task GetAllAsync_WithNoProducts_ShouldReturnEmptyList()
    {
        var result = await _repository.GetAllAsync();

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task GetByIdAsync_WithExistingId_ShouldReturnProduct()
    {
        var product = new Product { Name = "Test Product", Category = "Electronics", Price = 100m, Quantity = 10 };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(product.Id);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Test Product"));
    }

    [Test]
    public async Task GetByIdAsync_WithNonExistingId_ShouldReturnNull()
    {
        var result = await _repository.GetByIdAsync(999);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddAsync_WithValidProduct_ShouldAddToDatabase()
    {
        var product = new Product { Name = "New Product", Category = "Electronics", Price = 100m, Quantity = 10 };

        var result = await _repository.AddAsync(product);

        Assert.That(result.Id, Is.GreaterThan(0));
        Assert.That(result.CreatedAt, Is.GreaterThan(DateTime.UtcNow.AddSeconds(-1)));

        var savedProduct = await _context.Products.FindAsync(result.Id);
        Assert.That(savedProduct, Is.Not.Null);
        Assert.That(savedProduct.Name, Is.EqualTo("New Product"));
    }

    [Test]
    public async Task UpdateAsync_WithExistingProduct_ShouldUpdateInDatabase()
    {
        var product = new Product { Name = "Original Name", Category = "Electronics", Price = 100m, Quantity = 10 };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        product.Name = "Updated Name";
        product.Price = 150m;
        product.UpdatedAt = DateTime.UtcNow;

        var result = await _repository.UpdateAsync(product);

        Assert.That(result.Name, Is.EqualTo("Updated Name"));
        Assert.That(result.Price, Is.EqualTo(150m));
        Assert.That(result.UpdatedAt, Is.Not.Null);

        var updatedProduct = await _context.Products.FindAsync(product.Id);
        Assert.That(updatedProduct.Name, Is.EqualTo("Updated Name"));
    }

    [Test]
    public async Task DeleteAsync_WithExistingId_ShouldRemoveFromDatabase()
    {
        var product = new Product { Name = "Product to Delete", Category = "Electronics", Price = 100m, Quantity = 10 };
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        var productId = product.Id;
        await _repository.DeleteAsync(productId);

        var deletedProduct = await _context.Products.FindAsync(productId);
        Assert.That(deletedProduct, Is.Null);
    }

    [Test]
    public async Task SearchAsync_WithMatchingName_ShouldReturnProducts()
    {
        var products = new List<Product>
        {
            new Product { Name = "Laptop Computer", Category = "Electronics", Price = 1000m, Quantity = 5 },
            new Product { Name = "Computer Mouse", Category = "Electronics", Price = 25m, Quantity = 20 },
            new Product { Name = "Desk Chair", Category = "Furniture", Price = 200m, Quantity = 10 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var result = await _repository.SearchAsync("computer");

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Any(p => p.Name.Contains("Computer")), Is.True);
    }

    [Test]
    public async Task SearchAsync_WithMatchingCategory_ShouldReturnProducts()
    {
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Category = "Electronics", Price = 1000m, Quantity = 5 },
            new Product { Name = "Mouse", Category = "Electronics", Price = 25m, Quantity = 20 },
            new Product { Name = "Chair", Category = "Furniture", Price = 200m, Quantity = 10 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var result = await _repository.SearchAsync("electronics");

        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.All(p => p.Category.Equals("Electronics", StringComparison.OrdinalIgnoreCase)), Is.True);
    }

    [Test]
    public async Task SearchAsync_WithNoMatches_ShouldReturnEmptyList()
    {
        var products = new List<Product>
        {
            new Product { Name = "Laptop", Category = "Electronics", Price = 1000m, Quantity = 5 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var result = await _repository.SearchAsync("nonexistent");

        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task SearchAsync_WithEmptySearchTerm_ShouldReturnAllProducts()
    {
        var products = new List<Product>
        {
            new Product { Name = "Product 1", Category = "Electronics", Price = 100m, Quantity = 10 },
            new Product { Name = "Product 2", Category = "Furniture", Price = 200m, Quantity = 5 }
        };

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        var result = await _repository.SearchAsync("");

        Assert.That(result.Count(), Is.EqualTo(2));
    }
}

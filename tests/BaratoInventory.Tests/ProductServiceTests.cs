using BaratoInventory.Core.Entities;
using BaratoInventory.Core.Interfaces;
using BaratoInventory.Infrastructure.Services;
using Moq;
using NUnit.Framework;

namespace BaratoInventory.Tests;

[TestFixture]
public class ProductServiceTests
{
    private Mock<IProductRepository> _mockRepository;
    private Mock<ICacheService> _mockCacheService;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IProductRepository>();
        _mockCacheService = new Mock<ICacheService>();
        _productService = new ProductService(_mockRepository.Object, _mockCacheService.Object);
    }

    [Test]
    public async Task GetAllProductsAsync_WhenCacheHit_ReturnsCachedProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Test Product", Category = "Test", Price = 10.99m, Quantity = 5 }
        };

        _mockCacheService.Setup(x => x.GetAsync<IEnumerable<Product>>(It.IsAny<string>()))
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.That(result, Is.EqualTo(expectedProducts));
        _mockRepository.Verify(x => x.GetAllAsync(), Times.Never);
    }

    [Test]
    public async Task GetAllProductsAsync_WhenCacheMiss_ReturnsProductsFromRepository()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Test Product", Category = "Test", Price = 10.99m, Quantity = 5 }
        };

        _mockCacheService.Setup(x => x.GetAsync<IEnumerable<Product>>(It.IsAny<string>()))
            .ReturnsAsync((IEnumerable<Product>)null);
        _mockRepository.Setup(x => x.GetAllAsync())
            .ReturnsAsync(expectedProducts);

        // Act
        var result = await _productService.GetAllProductsAsync();

        // Assert
        Assert.That(result, Is.EqualTo(expectedProducts));
        _mockRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _mockCacheService.Verify(x => x.SetAsync(It.IsAny<string>(), expectedProducts, It.IsAny<TimeSpan>()), Times.Once);
    }

    [Test]
    public async Task CreateProductAsync_WhenValidProduct_CallsRepositoryAndInvalidatesCache()
    {
        // Arrange
        var product = new Product { Name = "New Product", Category = "Test", Price = 15.99m, Quantity = 10 };

        _mockRepository.Setup(x => x.AddAsync(product))
            .ReturnsAsync(product);

        // Act
        var result = await _productService.CreateProductAsync(product);

        // Assert
        Assert.That(result, Is.EqualTo(product));
        _mockRepository.Verify(x => x.AddAsync(product), Times.Once);
        _mockCacheService.Verify(x => x.RemoveByPatternAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task UpdateProductAsync_WhenProductExists_UpdatesProductAndInvalidatesCache()
    {
        // Arrange
        var existingProduct = new Product { Id = 1, Name = "Old Name", Category = "Test", Price = 10.99m, Quantity = 5 };
        var updatedProduct = new Product { Id = 1, Name = "New Name", Category = "Test", Price = 12.99m, Quantity = 8 };

        _mockRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingProduct);
        _mockRepository.Setup(x => x.UpdateAsync(It.IsAny<Product>()))
            .ReturnsAsync(existingProduct);

        // Act
        var result = await _productService.UpdateProductAsync(1, updatedProduct);

        // Assert
        Assert.That(result.Name, Is.EqualTo("New Name"));
        Assert.That(result.Price, Is.EqualTo(12.99m));
        Assert.That(result.Quantity, Is.EqualTo(8));
        _mockRepository.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Once);
        _mockCacheService.Verify(x => x.RemoveByPatternAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void UpdateProductAsync_WhenProductNotFound_ThrowsArgumentException()
    {
        // Arrange
        var product = new Product { Name = "Test Product", Category = "Test", Price = 10.99m, Quantity = 5 };

        _mockRepository.Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Product)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await _productService.UpdateProductAsync(999, product));
        Assert.That(ex.Message, Does.Contain("Product with ID 999 not found"));
    }

    [Test]
    public async Task DeleteProductAsync_WhenValidId_CallsRepositoryAndInvalidatesCache()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Test Product", Category = "Test", Price = 10.99m, Quantity = 5 };

        _mockRepository.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(product);

        // Act
        await _productService.DeleteProductAsync(1);

        // Assert
        _mockRepository.Verify(x => x.DeleteAsync(1), Times.Once);
        _mockCacheService.Verify(x => x.RemoveByPatternAsync(It.IsAny<string>()), Times.Once);
    }
}

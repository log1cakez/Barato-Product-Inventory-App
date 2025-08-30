using BaratoInventory.Core.Entities;
using BaratoInventory.Core.Interfaces;

namespace BaratoInventory.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICacheService _cacheService;
    private const string CacheKey = "products_all";
    private const string CachePattern = "products_*";

    public ProductService(IProductRepository productRepository, ICacheService cacheService)
    {
        _productRepository = productRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        // Try to get from cache first
        var cachedProducts = await _cacheService.GetAsync<IEnumerable<Product>>(CacheKey);
        if (cachedProducts != null)
            return cachedProducts;

        // If not in cache, get from database
        var products = await _productRepository.GetAllAsync();
        
        // Cache the result for 5 minutes
        await _cacheService.SetAsync(CacheKey, products, TimeSpan.FromMinutes(5));
        
        return products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        var createdProduct = await _productRepository.AddAsync(product);
        
        // Invalidate cache
        await _cacheService.RemoveByPatternAsync(CachePattern);
        
        return createdProduct;
    }

    public async Task<Product> UpdateProductAsync(int id, Product product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);
        if (existingProduct == null)
            throw new ArgumentException($"Product with ID {id} not found");

        existingProduct.Name = product.Name;
        existingProduct.Category = product.Category;
        existingProduct.Price = product.Price;
        existingProduct.Quantity = product.Quantity;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
        
        // Invalidate cache
        await _cacheService.RemoveByPatternAsync(CachePattern);
        
        return updatedProduct;
    }

    public async Task DeleteProductAsync(int id)
    {
        await _productRepository.DeleteAsync(id);
        
        // Invalidate cache
        await _cacheService.RemoveByPatternAsync(CachePattern);
    }

    public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
    {
        return await _productRepository.SearchAsync(searchTerm);
    }
}

using BaratoInventory.Core.Entities;

namespace BaratoInventory.Core.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(int id, Product product);
    Task DeleteProductAsync(int id);
    Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
}

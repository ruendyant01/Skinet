using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsAsync(string? brand, string? type, string? sort);
    Task<Product?> GetProductByIdAsync(int id);
    
    Task<IEnumerable<string>> GetBrandsAsync();
    Task<IEnumerable<string>> GetTypesAsync();
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    Task<bool> ProductExistAsync(Product product);
    Task<bool> SaveChangesAsync();
}
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRepository(StoreContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(brand))
            return query.Where(p => p.Brand == brand);

        if (!string.IsNullOrEmpty(type))
            return query.Where(p => p.Type == type);

        if (!string.IsNullOrEmpty(sort))
        {
            switch (sort)
            {
                case "priceAsc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    query = query.OrderByDescending(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }
        }
        
        return await query.ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<string>> GetBrandsAsync()
    {
        return await context.Products.Select(p => p.Brand).Distinct().ToListAsync();
    }

    public async Task<IEnumerable<string>> GetTypesAsync()
    {
        return await context.Products.Select(p => p.Type).Distinct().ToListAsync();
    }

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<bool> ProductExistAsync(Product product)
    {
        return await context.Products.AnyAsync(p => p.Id == product.Id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
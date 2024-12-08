using System.Text.Json;
using Core.Entities;
using Infrastructure.Data;

namespace Infrastructure.Seed;

public class ProductSeed
{
    public static async Task SeedAsync(StoreContext storeContext)
    {
        var rawData = await File.ReadAllTextAsync("../Infrastructure/Seed/Data/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(rawData);
        
        if (products is null) return;
        
        storeContext.Products.AddRange(products);
        await storeContext.SaveChangesAsync();
    }
}
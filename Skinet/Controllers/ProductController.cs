using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Skinet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(StoreContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await context.Products.ToListAsync();
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await context.Products.FindAsync(id);
        
        if(product == null) return NotFound();
        
        return product;
    }
    
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProductById), new {id = product.Id}, product);
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        var existingProduct = await context.Products.FindAsync(id);
        
        if(existingProduct == null) return NotFound();
        
        context.Entry(product).State = EntityState.Modified;
        
        await context.SaveChangesAsync();
        
        return NoContent();
    }
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        
        if(product == null) return NotFound();
        
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        
        return NoContent();
    }
}
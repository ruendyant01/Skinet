using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Skinet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductRepository productRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts(string? brand, string? type, string? sort)
    {
        return await productRepository.GetProductsAsync(brand, type, sort);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.AddProduct(product);
        if (await productRepository.SaveChangesAsync())
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

        return BadRequest("Failed to create product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Product>> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !await productRepository.ProductExistAsync(product))
            return BadRequest("Product id does not match route id");

        productRepository.UpdateProduct(product);

        if (await productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to update product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Product>> DeleteProduct(int id)
    {
        var product = await productRepository.GetProductByIdAsync(id);

        if (product == null) return NotFound();

        productRepository.DeleteProduct(product);

        if (await productRepository.SaveChangesAsync())
            return NoContent();

        return BadRequest("Failed to delete product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<string>>> GetBrands()
    {
        return Ok(await productRepository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<string>>> GetTypes()
    {
        return Ok(await productRepository.GetTypesAsync());
    }
}
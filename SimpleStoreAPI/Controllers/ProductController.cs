using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleStoreAPI.DTOs.Products;
using SimpleStoreAPI.Interfaces;

namespace SimpleStoreAPI.Controllers;

[Route("api/products")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        var newProduct = await _productService.CreateAsync(productDto);

        if (!newProduct.Succeeded)
        {
            return BadRequest(newProduct.Errors);
        }

        return CreatedAtAction(nameof(GetById), new
        {
            id = newProduct.Data!.Id
        }, newProduct.Data);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (!product.Succeeded)
        {
            return NotFound(product.Errors);
        }

        return Ok(product.Data);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateProductDto productDto)
    {
        var updatedProduct = await _productService.UpdateAsync(id, productDto);

        if (!updatedProduct.Succeeded)
        {
            return BadRequest(updatedProduct.Errors);
        }

        return Ok(updatedProduct.Data);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var deletedProduct = await _productService.DeleteAsync(id);

        if (!deletedProduct.Succeeded)
        {
            return BadRequest(deletedProduct.Errors);
        }

        return NoContent();
    }

}

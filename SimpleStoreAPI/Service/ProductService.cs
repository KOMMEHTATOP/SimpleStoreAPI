using Microsoft.EntityFrameworkCore;
using SimpleStoreAPI.Data;
using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.Products;
using SimpleStoreAPI.Interfaces;
using SimpleStoreAPI.Mappers;

namespace SimpleStoreAPI.Service;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;
    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ProductResponseDto>> CreateAsync(CreateProductDto productDto)
    {
        var existProduct = await _context.Products.AnyAsync(p => p.Name == productDto.Name);

        if (existProduct)
        {
            return Result<ProductResponseDto>.Failed("Product already exists");
        }

        var newProduct = ProductMapper.CreateProductFromDto(productDto);
        newProduct.CreatedAt = DateTime.Now;
        newProduct.UpdatedAt = DateTime.Now;
        
        _context.Add(newProduct);
        await _context.SaveChangesAsync();

        var result = ProductMapper.ProductToResponseDto(newProduct);
        return Result<ProductResponseDto>.Success(result);
    }

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    public Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
    public Task<Result<ProductResponseDto>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
    public Task<Result<ProductResponseDto>> UpdateAsync(string id, UpdateProductDto productDto)
    {
        throw new NotImplementedException();
    }
    public Task<Result> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}

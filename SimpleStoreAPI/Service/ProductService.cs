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
    private readonly ICurrentUserService _currentUserService;
    public ProductService(ApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<Result<ProductResponseDto>> CreateAsync(CreateProductDto productDto)
    {
        var userId = _currentUserService.GetUserId();
        Console.WriteLine($"ProductService - UserId from CurrentUserService: '{userId}'");
        Console.WriteLine($"ProductService - UserId is null or empty: {string.IsNullOrEmpty(userId)}");

        if (string.IsNullOrEmpty(userId))
        {
            return Result<ProductResponseDto>.Failed("User not authenticated");
        }

        var existProduct = await _context.Products.AnyAsync(p => p.Name == productDto.Name && p.SellerId == userId);

        if (existProduct)
        {
            return Result<ProductResponseDto>.Failed("You already have a product with this name");
        }

        var newProduct = ProductMapper.CreateProductFromDto(productDto);
        newProduct.CreatedAt = DateTime.UtcNow;
        newProduct.UpdatedAt = DateTime.UtcNow;
        newProduct.SellerId = userId;
        _context.Add(newProduct);
        await _context.SaveChangesAsync();
        
        var createProduct = await _context.Products
            .Include(p => p.Seller)
            .FirstAsync(p=>p.Id == newProduct.Id);

        var result = ProductMapper.ProductToResponseDto(createProduct);
        return Result<ProductResponseDto>.Success(result);
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _context.Products
            .Include(p => p.Seller)
            .AsNoTracking()
            .ToListAsync();
        return products.Select(ProductMapper.ProductToResponseDto).ToList();
    }


    public async Task<Result<ProductResponseDto>> GetByIdAsync(string id)
    {
        var existProduct = await _context.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existProduct == null)
        {
            return Result<ProductResponseDto>.Failed("Product not found");
        }

        return Result<ProductResponseDto>.Success(ProductMapper.ProductToResponseDto(existProduct));
    }

    public async Task<Result<ProductResponseDto>> UpdateAsync(string id, UpdateProductDto productDto)
    {
        var currentUserId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Result<ProductResponseDto>.Failed("User not authenticated");
        }

        var existProduct = await _context.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (existProduct == null)
        {
            return Result<ProductResponseDto>.Failed("Product not found");
        }

        var isOwner = currentUserId == existProduct.SellerId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (!isOwner && !isAdmin)
        {
            return Result<ProductResponseDto>.Failed("You are not authorized to modify this product");
        }

        ProductMapper.UpdateExistingProductFromDto(existProduct, productDto);
        existProduct.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Result<ProductResponseDto>.Success(ProductMapper.ProductToResponseDto(existProduct));
    }

    public async Task<Result> DeleteAsync(string id)
    {
        var currentUserId = _currentUserService.GetUserId();

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Result.Failed("User not authenticated");
        }

        var existProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

        if (existProduct == null)
        {
            return Result.Failed("Product not found");
        }

        var isOwner = currentUserId == existProduct.SellerId;
        var isAdmin = _currentUserService.IsInRole("Admin");

        if (!isOwner && !isAdmin)
        {
            return Result.Failed("You are not authorized to modify this product");
        }

        _context.Remove(existProduct);
        await _context.SaveChangesAsync();
        return Result.Success();
    }
}

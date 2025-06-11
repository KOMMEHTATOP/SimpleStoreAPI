using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.Products;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Mappers;

public static class ProductMapper
{
    public static Product CreateProductFromDto(CreateProductDto createProductDto)
    {
        return new Product
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Category = createProductDto.Category,
            Stock = createProductDto.Stock
        };
    }

    public static ProductResponseDto ProductToResponseDto(Product product)
    {
        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            Stock = product.Stock
        };
    }

    public static void UpdateExistingProductFromDto(Product existingProduct, UpdateProductDto dto)
    {
        existingProduct.Name = dto.Name;
        existingProduct.Description = dto.Description;
        existingProduct.Price = dto.Price;
        existingProduct.Stock = dto.Stock;
        existingProduct.Category = dto.Category;
    }
}

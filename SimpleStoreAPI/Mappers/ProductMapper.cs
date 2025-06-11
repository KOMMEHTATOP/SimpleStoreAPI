using SimpleStoreAPI.DTOs.Products;
using SimpleStoreAPI.Models;

namespace SimpleStoreAPI.Mappers;

public class ProductMapper
{
    public static ProductResponseDto CreateResponseDto(CreateProductDto createProductDto)
    {
        return new ProductResponseDto
        {
            Name = createProductDto.Name,
            Description = createProductDto.Description,
            Price = createProductDto.Price,
            Category = createProductDto.Category,
            Stock = createProductDto.Stock
        };
    }

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
}

using SimpleStoreAPI.DTOs;
using SimpleStoreAPI.DTOs.Products;

namespace SimpleStoreAPI.Interfaces;

public interface IProductService
{
    Task<Result<ProductResponseDto>> CreateAsync(CreateProductDto productDto);
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<Result<ProductResponseDto>> GetByIdAsync(string id);
    Task<Result<ProductResponseDto>> UpdateAsync(string id, UpdateProductDto productDto);
    Task<Result> DeleteAsync(string id);
}

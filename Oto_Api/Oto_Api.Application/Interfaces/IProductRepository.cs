using Oto_Api.Application.DTOs.ProductDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetProductPagedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountProductAsync();
        Task<List<Products>> GetProductAsync();
        Task<ProductDto?> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync( ProductDto product);
        Task<bool> EditProductAsync(int id, ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
        Task<List<ProductDto>> SearchProductAsync(string searchTerm, int pageNumber, int pageSize);
        Task<List<Products>> GetProductFullOptionAsync();
        Task<int> CountProductAsync(string searchTerm);
    }
}

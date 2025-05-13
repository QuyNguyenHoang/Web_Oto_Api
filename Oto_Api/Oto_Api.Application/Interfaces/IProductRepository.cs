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
        Task<List<Products>> GetProductPagedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountProductAsync();
        Task<Products> GetProductByIdAsync(int id);
        Task<bool> CreateProductAsync(ProductDto product);
        Task<bool> EditProductAsync(int id, ProductDto productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}

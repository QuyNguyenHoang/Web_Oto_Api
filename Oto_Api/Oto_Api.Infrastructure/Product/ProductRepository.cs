using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.ProductDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Product
{
    public class ProductRepository : IProductRepository
    {
       
        private readonly ApplicationDbContext _context;
        private readonly ICategoryRepository _category;
        public ProductRepository( ApplicationDbContext context, ICategoryRepository category)
        {
            
            _context = context;
            _category = category;
        }
        public async Task<List<Products>> GetProductPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> GetTotalCountProductAsync()
        {
            return await _context.Products.CountAsync();
        }
        public async Task<Products?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
        public async Task<bool> CreateProductAsync( ProductDto productDto)
        {
            try
            {

                var productName = await _context.Products
                    .Where(p => p.ProductName == productDto.ProductName)
                    .FirstOrDefaultAsync();
                if (productName != null)
                {
                    return false;
                }
                var newProduct = new Products
                {
                    ProductName = productDto.ProductName,
                    Description = productDto.Description,
                    CategoryId = productDto.CategoryId,

                };
                _context.Products.Add(newProduct);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }

        }
        public async Task<bool> EditProductAsync(int id, ProductDto productDto)
        {
            try
            {
                var productById = await _context.Products.FindAsync(id);
                if (productById == null)
                {
                    return false;
                }
                
                    productById.ProductName = productDto.ProductName;
                    productById.Description = productDto.Description;
                    productById.CategoryId = productDto.CategoryId;
                
                _context.Products.Update(productById);
                var result = await _context.SaveChangesAsync();
                return result > 0;


            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var ProductId = await _context.Products.FindAsync(id);
                if (ProductId == null)
                { return false; }
                _context.Remove(ProductId);
                var deleteResult = await _context.SaveChangesAsync();
                return deleteResult > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }

        }
        public async Task<List<Products>> SearchProductAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _context.Products
                .Include(p=>p.Categories)
                .Where(p=>
                (!string.IsNullOrEmpty(p.ProductName)) && p.ProductName.ToLower().Contains(searchTerm.ToLower())||(!string.IsNullOrEmpty(p.Categories.CategoryName)) && p.Categories.CategoryName.ToLower().Contains(searchTerm.ToLower()))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

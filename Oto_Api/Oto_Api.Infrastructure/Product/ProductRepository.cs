using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Application.DTOs.PriceDTOs;
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
        public ProductRepository(ApplicationDbContext context, ICategoryRepository category)
        {

            _context = context;
            _category = category;
        }

        public async Task<List<ProductDto>> GetProductPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Products
    .Include(p => p.Pictures)
    .Include(p => p.Prices)
    .OrderBy(p => p.ProductId)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .Select(p => new ProductDto
    {
        ProductId = p.ProductId,
        ProductName = p.ProductName,
        Description = p.Description,
        CreatedDate = p.CreatedDate,
        IsAvailable = p.IsAvailable,
        CategoryId = p.CategoryId,
        Pictures = p.Pictures.Select(pic => new PictureDto
        {
            ImageUrl = pic.ImageUrl
        }).ToList(),
        Prices = p.Prices.OrderByDescending(price => price.EffectiveDate).Select(price => price.PriceSale).FirstOrDefault()
    })
    .ToListAsync();

        }
        public async Task<List<Products>> GetProductAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> GetTotalCountProductAsync()
        {
            return await _context.Products.CountAsync();
        }
        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Pictures)
                .Include(p => p.Categories)
                .Where(p => p.ProductId == id)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    CategoryId = p.CategoryId,
                    Pictures = p.Pictures.Select(pic => new PictureDto
                    {
                        ImageUrl = pic.ImageUrl
                    }).ToList(),
                    Prices = p.Prices.OrderByDescending(price => price.EffectiveDate).Select(price => price.PriceSale).FirstOrDefault()
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<Products>> GetProductFullOptionAsync()
        {
            return await _context.Products
                .Include(p => p.Pictures)
                .Include(p => p.Categories)
                .ToListAsync();
        }

        public async Task<bool> CreateProductAsync(ProductDto productDto)
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
        public async Task<List<ProductDto>> SearchProductAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Pictures)
                .Include(p => p.Prices)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchTerm));
            }

            return await query
                .OrderBy(p => p.ProductId) // hoặc theo trường nào đó phù hợp để ổn định thứ tự
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    CreatedDate = p.CreatedDate,
                    IsAvailable = p.IsAvailable,
                    CategoryId = p.CategoryId,
                    Pictures = p.Pictures.Select(pic => new PictureDto
                    {
                        ImageUrl = pic.ImageUrl
                    }).ToList(),
                    Prices = p.Prices.OrderByDescending(price => price.EffectiveDate)
                                      .Select(price => price.PriceSale)
                                      .FirstOrDefault()
                })
                .ToListAsync();
        }
    
        public async Task<int> CountProductAsync(string searchTerm)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c => c.ProductName.ToLower().Contains(searchTerm));
            }

            return await query.CountAsync();
        }
    }
}

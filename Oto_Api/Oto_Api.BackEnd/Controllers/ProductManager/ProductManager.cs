using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Application.DTOs.ProductDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.Product;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Oto_Api.BackEnd.Controllers.ProductManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductManager : Controller
    {
        private readonly IProductRepository _repository;
        public ProductManager(IProductRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("GetAll_Product")]
        public async Task<IActionResult> GetAllProduct(int pageNumber = 1, int pageSize = 10)
        {
            var ProductData = await _repository.GetProductPagedAsync(pageNumber, pageSize);
            var ProductTotal = await _repository.GetTotalCountProductAsync();
            return Ok(new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = ProductTotal,
                totalPages = (int)Math.Ceiling((double)ProductTotal / pageSize),
                data = ProductData

            });

        }
        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Map sang DTO nếu cần, hoặc trả thẳng
            var productDto = new
            {
                product.ProductId,
                product.ProductName,
                product.Description,
                product.CreatedDate,
                product.IsAvailable,
                product.CategoryId,
                CategoryName = product.Categories?.CategoryName,
                Pictures = product.Pictures?.Select(pic => new
                {
                    pic.PictureId,
                    pic.ImageUrl,
                }),
               
            };

            return Ok(productDto);
        }
        [HttpPost("Create_Product")]
        public async Task<IActionResult> CreateProduct( [FromBody] ProductDto productDto)
        {
            var CreateProduct = await _repository.CreateProductAsync( productDto);
            if (!CreateProduct)
            {
                return BadRequest(new { error = true, message = "Product already exists" });
            }
            else
            {
                return Ok(new { successfullt = true, message = "Create product successfully!" });
            }
        }
        [HttpPut("Edit_Product")]
        public async Task<IActionResult> EditProduct(int id, [FromBody] ProductDto productDto)
        {
            var getProduct = await _repository.GetProductByIdAsync(id);
            if (getProduct == null)
            {
                return BadRequest(new { error = true, message = $"Not found product with id = {id}" });
            }
            bool EditProduct = await _repository.EditProductAsync(id,productDto);
            if (!EditProduct)
            {
                return BadRequest(new { error = true, message = $"Can not Update Product with id = {id}" });
            }
            else
            {
                return Ok(new { successfully = true, message = $"Update Product is successfully!" });
            }
        }
        [HttpDelete("Delete_Product")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleteProduct = await _repository.DeleteProductAsync(id);
            if (!deleteProduct)
            {
                return BadRequest(new { error = true, message = $"Can not delete Product with id = {id}" });
            }
            else
            {
                return Ok(new { successfully = true, message = $"Delete Product with id = {id} is successfully!" });
            }
        }
        [HttpGet("Search_Product")]
        public async Task<IActionResult> SearchProduct(string searchTerm, int pageNumber= 1, int pageSize = 5)
        {
            var searchResult = await _repository.SearchProductAsync(searchTerm, pageNumber, pageSize);
            if (searchResult == null || !searchResult.Any())
            {
                return NotFound(new { message = " Not found Product" });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"This is the result for the product searched with the keyword = {searchTerm}",
                    data = searchResult
                });
            }
        }
    }
}

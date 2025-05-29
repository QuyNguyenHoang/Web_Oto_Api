using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Application.DTOs.ProductDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.Category;
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
            var productData = await _repository.GetProductPagedAsync(pageNumber, pageSize);
            var productTotal = await _repository.GetTotalCountProductAsync();

            return Ok(new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = productTotal,
                totalPages = (int)Math.Ceiling((double)productTotal / pageSize),
                data = productData
            });
        }
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _repository.GetProductAsync();
            

            return Ok(products);
        }


        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

           
            var productById = await _repository.GetProductByIdAsync(id);

            return Ok(new
            {
                successfully = true,
                data = productById
            });
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
        [HttpPut("Edit_Product/{id}")]
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
        [HttpDelete("Delete_Product/{id}")]
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
        public async Task<IActionResult> SearchProduct(string searchTerm = "", int pageNumber= 1, int pageSize = 5)
        {
            var products = await _repository.SearchProductAsync(searchTerm, pageNumber, pageSize);
            var totalCount = await _repository.CountProductAsync(searchTerm);

            if (products == null || !products.Any())
            {
                return NotFound(new { message = "No product found" });
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(new
            {
                data = products,
                currentPage = pageNumber,

                pageSize = pageSize,
                totalRecords = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),

            });
        }
    }
    
}

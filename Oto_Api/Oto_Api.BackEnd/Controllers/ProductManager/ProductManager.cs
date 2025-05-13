using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Application.DTOs.ProductDTOs;
using Oto_Api.Application.Interfaces;
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
        [HttpGet("Create_Product")]
        public async Task<IActionResult> CreateProduct(int id, [FromBody] ProductDto productDto)
        {
            var CreateProduct = await _repository.CreateProductAsync(productDto);
            if (!CreateProduct)
            {
                return BadRequest(new { error = true, message = "Product already exists" });
            }
            else
            {
                return Ok(new { successfullt = true, message = "Create product successfully!" });
            }
        }
        [HttpPost("Edit_Product")]
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
    }
}

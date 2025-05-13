using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.StockDTOs;
using Oto_Api.Application.Interfaces;

namespace Oto_Api.BackEnd.Controllers.StockManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockManager : Controller
    {
        private readonly IStockRepository _stockRepository;
        private readonly IProductRepository _productRepository;
        public StockManager(IStockRepository stockRepository, IProductRepository productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }
        [HttpGet("GetAll_Stock")]
        public async Task<IActionResult> GetAllStock(int pageNumber = 1, int pageSize = 10)
        {
            var StockData = await _stockRepository.GetStockPageAsync(pageNumber, pageSize);
            var StockCount = await _stockRepository.TotalCountStockAsync();
            return Ok(new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = StockCount,
                totalPage = (int)Math.Ceiling((double)StockCount / pageSize),
                data = StockData
            }
            );
        }
        [HttpGet("GetStockById")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stockById = await _stockRepository.GetStockByIdAsync(id);
            if (stockById == null)
            {
                return BadRequest(new
                {
                    error = true,
                    message = $"Not found Stock with id = {id}"
                });
            }
            return Ok(new
            {
                successfully = true,
                message = $"This is stock with id = {id}",
                data = stockById
            });
        }
        [HttpPost("Create_Stock")]
        public async Task<IActionResult> CreateStock([FromBody] StockDto stockDto)
        {
            var newStock = await _stockRepository.CreateStockAsync(stockDto);
            if (!newStock)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Create Stock"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"Create new Stock is successfully!"
                });
            }
        }
        [HttpPut("Update_Stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] StockDto stockDto)
        {
            var updateStock = await _stockRepository.UpdateStockAsync(id, stockDto);
            if (!updateStock)
            {
                return BadRequest(new
                {
                    error = true,
                    message = $"Can not Update Stock with id = {id}"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"Update  Stock with id = {id} is successfully!"
                });
            }
        }
        [HttpDelete("Delete_Stock")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var deleteStock = await _stockRepository.DeleteStockAsync(id);
            if (!deleteStock)
            {
                return BadRequest(new
                {
                    error = true,
                    message = $"Can not Delete Stock with id = {id}"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"Delete  Stock with id = {id} is successfully!"
                });
            }
        }
        [HttpGet("Search_Stock")]
        public async Task<IActionResult> SeaechStock(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var searchResult = await _stockRepository.SearchStockAsync(searchTerm, pageNumber, pageSize);
            if (searchResult == null || !searchResult.Any())
            {
                return NotFound(new { message = " Not found Stock" });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"This is the result for the Stock searched with the keyword = {searchTerm}",
                    data = searchResult
                });
            }
        }
    }
}



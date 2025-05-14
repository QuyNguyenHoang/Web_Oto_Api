using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Services;
using Oto_Api.Application.DTOs.PriceDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.Data;

namespace Oto_Api.BackEnd.Controllers.PriceManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriceManager : Controller
    {
        private readonly IPriceRepository _priceRepository;
        public PriceManager(IPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }
        [HttpGet("GetAll_Price")]
        public async Task<IActionResult> GetAllPrice(int pageNumber = 1, int pageSize = 10)
        {
            var priceData = await _priceRepository.GetPricePageAsync(pageNumber, pageSize);
            var priceCount = await _priceRepository.PriceCountAsync();
            return Ok(new 
            {
                pageCurrent = pageNumber,
                pageSize = pageSize,
                totalRecords = priceCount,
                totalPage =(int)Math.Ceiling((double)priceCount/pageSize),
                data = priceData,

            });
        }
        [HttpGet("GetPriceById")]
        public async Task<IActionResult> GetPriceById(int id)
        {
            var priceById = await _priceRepository.GetPriceByIdAsync(id);
            if (priceById == null)
            {
                return NotFound(new
                {
                    error = true,
                    message = $"Not found Price with id = {id}"
                });
            }
            else
            {
                return Ok(new
                {
                    successfullt = true,
                    message = $"This is Price with id= {id}",
                    data = priceById,
                });
            }
        }
        [HttpPost("Create_Price")]
        public async Task<IActionResult> CreatePrice([FromBody] PriceDto priceDto)
        {
            bool createPrice =  await _priceRepository.CreatePriceAsync(priceDto);
            if (!createPrice)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Create price!",
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Create price is successfully!",
                    data = priceDto,
                });
            }
        }
        [HttpPut("Update_Price")]
        public async Task<IActionResult> UpdatePrice(int id,[FromBody] PriceDto priceDto)
        {
            bool updatePrice = await _priceRepository.UpdatePriceAsync(id,priceDto);
            if (!updatePrice)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Update price!",
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Update price is successfully!",
                    data = priceDto,
                });
            }
        }
        [HttpDelete("Delete_Price")]
        public async Task<IActionResult>DeletePrice(int id)
        {
            bool deletePrice = await _priceRepository.DeletePriceAsync(id);
            if (!deletePrice)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Delete price!",
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Delete Price is successfully!",
                    data = deletePrice,
                });
            }
        }
        [HttpGet("Search_Price")]
        public async Task<IActionResult> SearchPrice(DateTime searchTerm, int pgaeNumber = 1, int pageSize = 10)
        {
            var searchResult = await _priceRepository.SearchPriceAsync(searchTerm, pgaeNumber, pageSize);
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

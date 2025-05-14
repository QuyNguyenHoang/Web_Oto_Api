using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.CartDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.Stock;

namespace Oto_Api.BackEnd.Controllers.USER.CartManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartManager : Controller
    {
        private readonly ICartRepository _cartRepository;
        public CartManager(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        [HttpGet("GetAll_Cart")]
        public async Task<IActionResult> GetAllCart(int pageNumber = 1, int pageSize = 10)
        {
            var cartData = await _cartRepository.GetCartPageAsync(pageNumber, pageSize);
            var cartCout = await _cartRepository.CountCartAsync();
            return Ok(new
            {
                pageCurrent = pageNumber,
                pageNumber = pageSize,
                totalRecords = cartCout,
                totalPages = (int)Math.Ceiling((double)cartCout / pageSize),
                data = cartData
            });
        }
        [HttpGet("GetCartByUser")]
        public async Task<IActionResult> GetCartByUser(string id, int pageNumber = 1, int pageSize = 10)
        {
            var cartDataByUser = await _cartRepository.GetCartByUserAsync(id, pageNumber, pageSize);
            var cartCoutByUser = await _cartRepository.CountCartAsync();
            return Ok(new
            {
                pageCurrent = pageNumber,
                pageNumber = pageSize,
                totalRecords = cartCoutByUser,
                totalPages = (int)Math.Ceiling((double)cartCoutByUser / pageSize),
                data = cartDataByUser
            });
        }
        [HttpPost("Create_Cart")]
        public async Task<IActionResult> CreateCart([FromBody] CartDto cartDto)
        {
            var createResult = await _cartRepository.CreateCartAsync(cartDto);
            if (!createResult)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Create new Cart"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Create is successfully!",
                    data = createResult,
                });
            }
        }
        [HttpPut("Update_Cart")]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] CartDto cartDto)
        {
            var updateCart = await _cartRepository.UpdateCartAsync(id, cartDto);
            if (!updateCart)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Update cart"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Update is successfully!",
                    data = updateCart,
                });
            }
        }
        [HttpDelete("Delete_Cart")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var deleteCart = await _cartRepository.DeleteCartAsync(id);
            if (!deleteCart)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Delete cart"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Delete is successfully!",
                    data = deleteCart,
                });
            }
        }
        [HttpGet("Search_Cart")]
        public async Task<IActionResult> SearchCart(DateTime searchTerm, int pageNumber = 1, int pageSize = 10)
        {
            var searchResult = await _cartRepository.SearchCartAsync(searchTerm, pageNumber, pageSize);
            if (searchResult == null || !searchResult.Any())
            {
                return NotFound(new { message = $" Not found Cart with day = {searchTerm}" });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"This is the result for the Cart searched with the keyword = {searchTerm}",
                    data = searchResult
                });
            }

        }

    }
}

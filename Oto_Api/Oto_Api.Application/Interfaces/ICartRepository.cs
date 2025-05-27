using Oto_Api.Application.DTOs.CartDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<List<Carts>> GetCartPageAsync(int pageNumber, int pageSize);
       Task<List<CartDto>> GetCartByUserAsync(string userId, int pageNumber, int pageSize);
        Task<int> CountCartAsync();
        Task<bool> CreateCartAsync(CartDto cartDto);
        Task<bool> UpdateCartAsync(int id, UpdateCartDto updatecartDto);
        Task<bool> DeleteCartAsync(int id);
        Task<List<Carts>> SearchCartAsync(DateTime searchTerm, int pageNumber = 1, int pageSize = 10);
         Task<CartDto?> GetCartItemByUserAndProductAsync(string userId, int productId);
        Task<bool> AddOrUpdateCartAsync(CartDto cartDto);
    }
}

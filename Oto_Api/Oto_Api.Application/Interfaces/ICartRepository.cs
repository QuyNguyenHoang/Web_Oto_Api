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
        Task<List<Carts>> GetCartByUserAsync(string id, int pageNumber, int pageSize);
        Task<int> CountCartAsync();
        Task<bool> CreateCartAsync(CartDto cartDto);
        Task<bool> UpdateCartAsync(int id, CartDto cartDto);
        Task<bool> DeleteCartAsync(int id);
        Task<List<Carts>> SearchCartAsync(DateTime searchTerm, int pageNumber = 1, int pageSize = 10);
    }
}

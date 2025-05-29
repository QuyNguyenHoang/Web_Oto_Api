using Oto_Api.Application.DTOs.OderDTO;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IOrderRepository
    {
       Task<bool> CreateOrderAsync(OrderDto dto);
        Task<List<Orders>> GetOrdersByUserIdAsync(string Id);
        Task<OrderDto> GetOrderWithDetailsAsync(int orderId);
    }
}

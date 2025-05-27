using Oto_Api.Application.DTOs.OderDTO;
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
    }
}

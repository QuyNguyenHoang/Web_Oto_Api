using Oto_Api.Application.DTOs.PriceDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IPriceRepository
    {
        Task<List<Prices>> SearchPriceAsync(DateTime searchTerm, int pageNumber = 1, int pageSize = 10);
        Task<bool> DeletePriceAsync(int id);
        Task<bool> UpdatePriceAsync(int id, PriceDto priceDto);
        Task<bool> CreatePriceAsync(PriceDto priceDto);
        Task<Prices?> GetPriceByIdAsync(int id);
        Task<List<Prices>> GetPricePageAsync(int pageNumber, int pageSize);
        Task<List<Prices>> GetAllPriceAsync();
        Task<int> PriceCountAsync();
       
    }
}

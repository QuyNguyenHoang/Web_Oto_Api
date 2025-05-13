using Oto_Api.Application.DTOs.StockDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stocks>> GetAllStockAsync();
        Task<List<Stocks>> GetStockPageAsync(int pageNumber, int pageSize);
        Task<int> TotalCountStockAsync();
        Task<bool> CreateStockAsync(StockDto stockDto);
        Task<bool> UpdateStockAsync(int id, StockDto stockDto);
        Task<Stocks?> GetStockByIdAsync(int id);
        Task<bool> DeleteStockAsync(int id);
        Task<List<Stocks>> SearchStockAsync(string searchTerm, int pageNumber, int pageSize);
    }
}

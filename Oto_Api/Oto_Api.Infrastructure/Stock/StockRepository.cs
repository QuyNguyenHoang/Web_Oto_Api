using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.StockDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Stock
{

    public class StockRepository : IStockRepository
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        public StockRepository(IProductRepository productRepository, ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }
        //get all
        public async Task<List<Stocks>> GetAllStockAsync()
        {
            var allStock = await _context.Stocks.ToListAsync();
            if (allStock == null || !allStock.Any())
            {
                return new List<Stocks>();
            }
            return allStock;
        }
        //get all with page
        public async Task<List<Stocks>> GetStockPageAsync(int pageNumber, int pageSize)
        {
            return await _context.Stocks
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> TotalCountStockAsync()
        {
            return await _context.Stocks.CountAsync();
        }
        public async Task<bool> CreateStockAsync(StockDto stockDto)
        {
            var checkStock = await _context.Stocks
                .Where(s => s.WarehouseLocation == stockDto.WarehouseLocation)
                .FirstOrDefaultAsync();
            if (checkStock != null)
            {
                return false;
            }
            var newStock = new Stocks
            {
                Quantity = stockDto.Quanity,
                WarehouseLocation = stockDto.WarehouseLocation,
                ProductId = stockDto.ProductId,
            };
            _context.Add(newStock);
            var createResult = _context.SaveChanges();
            return createResult > 0;
        }
        public async Task<Stocks?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<bool> UpdateStockAsync(int id, StockDto stockDto)
        {
            var updateStock = await _context.Stocks.FindAsync(id);
            if (updateStock == null)
            {
                return false;
            }
            updateStock.Quantity = stockDto.Quanity;
            updateStock.WarehouseLocation = stockDto.WarehouseLocation;
            updateStock.ProductId = updateStock.ProductId;
            _context.Update(updateStock);
            var updateResult = _context.SaveChanges();
            return updateResult > 0;
        }
        public async Task<bool> DeleteStockAsync(int id)
        {
            var deleteStock = await _context.Stocks.FindAsync(id);
            if (deleteStock == null)
            {
                return false;
            }
            _context.Remove(deleteStock);
            var deleteResult = _context.SaveChanges();
            return deleteResult > 0;
        }
        public async Task<List<Stocks>> SearchStockAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _context.Stocks
                .Where(s =>
                (!string.IsNullOrEmpty(s.WarehouseLocation) &&
                s.WarehouseLocation.ToLower().Contains(searchTerm) ||
                (!string.IsNullOrEmpty(s.Product.ProductName) &&
                s.Product.ProductName.ToLower().Contains(searchTerm)))
                )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

    }
}

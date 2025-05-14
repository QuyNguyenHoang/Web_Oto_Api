using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.PriceDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Price
{
    public class PriceRepository : IPriceRepository
    {
        private readonly ApplicationDbContext _context;
        public PriceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Prices>> GetAllPriceAsync()
        {
            var allPrice = await _context.Prices.ToListAsync();
            if (allPrice == null || !allPrice.Any())
            {
                return new List<Prices>();
            }
            return allPrice;
        }
        public async Task<int> PriceCountAsync()
        {
            return await _context.Prices.CountAsync();
        }
        public async Task<List<Prices>> GetPricePageAsync(int pageNumber, int pageSize)
        {
            return await _context.Prices
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Prices?> GetPriceByIdAsync(int id)
        {
            return await _context.Prices.FindAsync(id);
        }
        public async Task<bool> CreatePriceAsync(PriceDto priceDto)
        {
            var checkPrice = await _context.Prices
                .Where(p => p.EffectiveDate == priceDto.EffectiveDate)
                .FirstOrDefaultAsync();
            if (checkPrice != null)
            {
                return false;

            }
            var newPrice = new Prices
            {
                PriceIn = priceDto.PriceIn,
                PriceSale = priceDto.PriceSale,
                EffectiveDate = priceDto.EffectiveDate,
                ProductId = priceDto.ProductId,
            };
            _context.Add(newPrice);
            var createResult = _context.SaveChanges();
            return createResult > 0;

        }
        public async Task<bool> UpdatePriceAsync(int id, PriceDto priceDto)
        {
            var updatePrice = await _context.Prices.FindAsync(id);
            if (updatePrice == null)
            {
                return false;
            }
            updatePrice.PriceIn = priceDto.PriceIn;
            updatePrice.PriceSale = priceDto.PriceSale;
            updatePrice.EffectiveDate = priceDto.EffectiveDate;
            updatePrice.ProductId = updatePrice.ProductId;
            _context.Update(updatePrice);
            var updateResult = _context.SaveChanges();
            return updateResult > 0;
        }
        public async Task<bool> DeletePriceAsync(int id)
        {
            var deletePrice = await _context.Prices.FindAsync(id);
                if (deletePrice == null)
            {
                return false;
            }
            _context.Remove(deletePrice);
            var deleteResult = _context.SaveChanges();
            return deleteResult > 0;
        }
        public async Task<List<Prices>> SearchPriceAsync(DateTime searchTerm, int pageNumber = 1, int pageSize = 10)
        {
          
                return await _context.Prices
                    .Where(p => p.EffectiveDate.Date == searchTerm) 
                    .OrderBy(p => p.EffectiveDate)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
           
        }

    }
}

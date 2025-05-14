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
        public async Task<List<Prices>> GetPricePageAsync(int pageNumber, int pageSize)
        {
            return await _context.Prices
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Prices?> GetPriceByIdAsync(int id)
        {
            return  await _context.Prices.FindAsync(id);
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
            };
            _context.Add(newPrice);
            var createResult =  _context.SaveChanges();
            return createResult > 0;

        }
        }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.CartDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Cart
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Carts>> GetCartPageAsync(int pageNumber, int pageSize)
        {
            return await _context.Carts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<List<Carts>> GetCartByUserAsync(string id, int pageNumber, int pageSize)
        {
            return await _context.Carts
                .Where(c => c.Id == id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<int> CountCartAsync()
        {
            return await _context.Carts.CountAsync();
        }
        public async Task<bool> CreateCartAsync(CartDto cartDto)
        {
            try
            {
                if (cartDto == null)
                {
                    return false;
                }
                var newCart = new Carts
                {
                    Id = cartDto.Id,
                    ProductId = cartDto.ProductId,
                    Price = cartDto.Price,
                    Quantity = cartDto.Quantity,
                    DateAdded = cartDto.DateAdded,
                    DateUpdated = cartDto.DateUpdated,
                    Status = cartDto.Status,
                    Discount = cartDto.Discount,
                    SessionId = cartDto.SessionId,
                    EstimatedDeliveryDate = cartDto.EstimatedDeliveryDate,
                    ShippingMethod = cartDto.ShippingMethod,

                };
                _context.Carts.Add(newCart);
                var createResult = await _context.SaveChangesAsync();
                return createResult > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<bool> UpdateCartAsync(int id, CartDto cartDto)
        {
            try
            {
                var getCart = await _context.Carts.FindAsync(id);
                if (getCart == null)
                {
                    return false;
                }
                else
                {
                    getCart.Id = getCart.Id;
                    getCart.ProductId = cartDto.ProductId;
                    getCart.Price = cartDto.Price;
                    getCart.Quantity = cartDto.Quantity;
                    getCart.DateAdded = cartDto.DateAdded;
                    getCart.DateUpdated = cartDto.DateUpdated;
                    getCart.Status = cartDto.Status;
                    getCart.Discount = cartDto.Discount;
                    getCart.SessionId = cartDto.SessionId;
                    getCart.EstimatedDeliveryDate = cartDto.EstimatedDeliveryDate;
                    getCart.ShippingMethod = cartDto.ShippingMethod;
                    _context.Update(getCart);
                    var updateResult = await _context.SaveChangesAsync();
                    return updateResult > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<bool> DeleteCartAsync(int id)
        {
            try
            {
                var idCart = await _context.Carts.FindAsync(id);
                if (idCart == null)
                {
                    return false;
                }
                else
                {
                    _context.Remove(idCart);
                    var deleteResult = await _context.SaveChangesAsync();
                    return deleteResult > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("",ex);
            }
        }
        
        public async Task<List<Carts>> SearchCartAsync(DateTime searchTerm, int pageNumber= 1, int pageSize=10)
        {
            return await _context.Carts
                .Where(c=>c.DateAdded.Date == searchTerm.Date )
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}

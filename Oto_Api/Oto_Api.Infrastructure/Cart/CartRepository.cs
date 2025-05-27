using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.CartDTOs;
using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Application.DTOs.ProductDTOs;
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
        public async Task<List<CartDto>> GetCartByUserAsync(string userId, int pageNumber, int pageSize)
        {
            return await _context.Carts
                .Where(c => c.Id == userId)
                .Include(c => c.Product)
                    .ThenInclude(p => p.Pictures)
                .Include(c => c.Product.Prices)
                .OrderBy(c => c.CartId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CartDto
                {
                    CartId = c.CartId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    Discount = c.Discount,
                    DateAdded = c.DateAdded,
                    DateUpdated = c.DateUpdated,
                    Status = c.Status,
                    SessionId = c.SessionId,
                    ShippingMethod = c.ShippingMethod,
                    EstimatedDeliveryDate = c.EstimatedDeliveryDate,

                   
                        ProductName = c.Product.ProductName, 
                       


                    Pictures = c.Product.Pictures.Select(pic => new PictureDto
                    {
                        ImageUrl = pic.ImageUrl,
                        ProductId = pic.ProductId
                    }).ToList(),

                    Prices = c.Product.Prices
                        .OrderByDescending(p => p.EffectiveDate)
                        .Select(p => p.PriceSale)
                        .FirstOrDefault()
                })
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
                    Id = cartDto.UserId,
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
        public async Task<bool> UpdateCartAsync(int id, UpdateCartDto cartDto)
        {
            try
            {
                var getCart = await _context.Carts.FindAsync(id);
                if (getCart == null)
                    return false;

                getCart.Quantity = cartDto.Quantity;
                getCart.Price = cartDto.Price;
                getCart.DateUpdated = DateTime.Now;

                _context.Carts.Update(getCart);
                var updateResult = await _context.SaveChangesAsync();

                return updateResult > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật giỏ hàng", ex);
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
        public async Task<CartDto?> GetCartItemByUserAndProductAsync(string userId, int productId)
        {
            var cartEntity = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == userId && c.ProductId == productId);

            if (cartEntity == null)
                return null;

            // Map entity -> DTO
            var cartDto = new CartDto
            {
                UserId = cartEntity.Id,
                ProductId = cartEntity.ProductId,
                Quantity = cartEntity.Quantity,
                Price = cartEntity.Price,
                Discount = cartEntity.Discount,
                SessionId = cartEntity.SessionId,
                ShippingMethod = cartEntity.ShippingMethod,
                EstimatedDeliveryDate = cartEntity.EstimatedDeliveryDate,
                DateAdded = cartEntity.DateAdded,
                DateUpdated = cartEntity.DateUpdated,
                Status = cartEntity.Status
            };

            return cartDto;
        }
        public async Task<bool> AddOrUpdateCartAsync(CartDto cartDto)
        {
            if ((string.IsNullOrEmpty(cartDto.UserId) && string.IsNullOrEmpty(cartDto.SessionId)) ||
                cartDto.ProductId <= 0 || cartDto.Quantity <= 0)
            {
                return false;
            }

            var existingCart = await _context.Carts.FirstOrDefaultAsync(c =>
                c.ProductId == cartDto.ProductId &&
                ((c.Id != null && c.Id == cartDto.UserId) ||
                 (c.SessionId != null && c.SessionId == cartDto.SessionId))
            );

            if (existingCart != null)
            {
                existingCart.Quantity += cartDto.Quantity;
                existingCart.Price = cartDto.Price ?? existingCart.Price;
                existingCart.Discount = cartDto.Discount ?? existingCart.Discount;
                existingCart.DateUpdated = DateTime.Now;
                existingCart.EstimatedDeliveryDate = cartDto.EstimatedDeliveryDate ?? existingCart.EstimatedDeliveryDate;
                existingCart.ShippingMethod = cartDto.ShippingMethod ?? existingCart.ShippingMethod;

                _context.Carts.Update(existingCart);
            }
            else
            {
                var newCart = new Carts
                {
                    Id = cartDto.UserId,
                    ProductId = cartDto.ProductId,
                    Quantity = cartDto.Quantity,
                    Price = cartDto.Price,
                    Discount = cartDto.Discount,
                    SessionId = cartDto.SessionId,
                    ShippingMethod = cartDto.ShippingMethod,
                    EstimatedDeliveryDate = cartDto.EstimatedDeliveryDate,
                    DateAdded = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Status = cartDto.Status ?? false
                };

                await _context.Carts.AddAsync(newCart);
            }

            return await _context.SaveChangesAsync() > 0;
        }



    }
}

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.PictureDTOs;
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
    public class PictureRepository : IPictureRepository
    {
        private readonly ApplicationDbContext _context;
        public PictureRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Pictures>> GetAllPictureAsync()
        {
            var allPicture= await _context.Pictures.ToListAsync();
            if (allPicture == null || !allPicture.Any())
            {
                return new List<Pictures>();
            }
            return allPicture;
        }
        public async Task<int> PictureCountAsync()
        {
            return await _context.Pictures.CountAsync();
        }
        public async Task<List<Pictures>> GetPricePageAsync(int pageNumber, int pageSize)
        {
            return await _context.Pictures
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Pictures?> GetPictureByIdAsync(int id)
        {
            return await _context.Pictures.FindAsync(id);
        }
        public async Task<bool> CreatePictureAsync(PictureDto pictureDto)
        {
            var checkUrl = await _context.Pictures
                .Where(p => p.ImageUrl == pictureDto.ImageUrl)
                .FirstOrDefaultAsync();
            if (checkUrl != null)
            {
                return false;

            }
            var newPicture = new Pictures
            {
                ImageUrl = pictureDto.ImageUrl,
                ProductId = pictureDto.ProductId,
            };
            _context.Add(newPicture);
            var createResult = _context.SaveChanges();
            return createResult > 0;

        }
        public async Task<bool> UpdatePictureAsync(int id, PictureDto picturesDto)
        {
            var updatePicture = await _context.Pictures.FindAsync(id);
            if (updatePicture == null)
            {
                return false;
            }
            updatePicture.ImageUrl = picturesDto.ImageUrl;
            updatePicture.ProductId = updatePicture.ProductId;
            _context.Update(updatePicture);
            var updateResult = _context.SaveChanges();
            return updateResult > 0;
        }
        public async Task<bool> DeletePictureAsync(int id)
        {
            var deletePicture = await _context.Pictures.FindAsync(id);
            if (deletePicture == null)
            {
                return false;
            }
            _context.Remove(deletePicture);
            var deleteResult = _context.SaveChanges();
            return deleteResult > 0;
        }
        public async Task<List<Pictures>> SearchPriceAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
        {

            return await _context.Pictures
                .Where(p => p.ImageUrl == searchTerm)
                .OrderBy(p => p.ImageUrl)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        }

    }
}

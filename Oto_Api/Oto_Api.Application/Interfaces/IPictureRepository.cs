using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IPictureRepository
    {
        Task<List<Pictures>> SearchPictureAsync(string searchTerm, int pageNumber = 1, int pageSize = 10);
        Task<bool> DeletePictureAsync(int id);
        Task<bool> UpdatePictureAsync(int id, PictureDto picturesDto);
        Task<bool> CreatePictureAsync(PictureDto pictureDto);
        Task<Pictures?> GetPictureByIdAsync(int id);
        Task<List<Pictures>> GetPicturePageAsync(int pageNumber, int pageSize);
        Task<int> PictureCountAsync(string searchTerm);
        Task<List<Pictures>> GetAllPictureAsync();
    }
}

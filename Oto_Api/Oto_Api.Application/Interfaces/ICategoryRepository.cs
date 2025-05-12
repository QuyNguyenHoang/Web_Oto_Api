using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
   public interface ICategoryRepository
    {
        Task<List<Categories>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<CategoriesDto> GetCategoryByIdAsync(int id);
        Task<bool> EditCategoryAsync(int id, CategoriesDto categoriesDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> CreateCategoryAsync(CategoriesDto categoriesDto);
        Task<List<Categories>> SearchCategoriesAsync(string searchTerm, int pageNumber, int pageSize);
    }
}

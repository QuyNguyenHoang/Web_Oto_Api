using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.Category
{

    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Categories>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Categories
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Categories.CountAsync();
        }

        public async Task<CategoriesDto?> GetCategoryByIdAsync(int id)
        {
            try
            {
                var CateById = await _context.Categories.FindAsync(id);
                if (CateById == null)
                {
                    return null;
                }
                return new CategoriesDto
                {
                    CategoryName = CateById.CategoryName,
                    CreatedDate = CateById.CreatedDate,
                    Description = CateById.Description,

                };
            }
            catch (Exception ex)
            {

                throw new Exception("An error occurred while fetching the category", ex);
            }

        }
        public async Task<bool> EditCategoryAsync(int id, CategoriesDto categoriesDto)
        {
            try
            {
                var editCate = await _context.Categories.FindAsync(id);
                if (editCate == null)
                {
                    return false;
                }
                editCate.CategoryName = categoriesDto.CategoryName;
                editCate.CreatedDate = DateTime.Now;
                editCate.Description = categoriesDto.Description;
                _context.Categories.Update(editCate);
                var result = await _context.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var DeleteCate = await _context.Categories.FindAsync(id);
                if (DeleteCate == null)
                {
                    return false;
                }
                _context.Categories.Remove(DeleteCate);
                var deleteResult = await _context.SaveChangesAsync();
                return deleteResult > 0;

            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<bool> CreateCategoryAsync(CategoriesDto categoriesDto)
        {
            try
            {
                var CateName = await _context.Categories
                    .Where(cate => cate.CategoryName == categoriesDto.CategoryName)
                    .FirstOrDefaultAsync();
                if (CateName != null)
                {
                    return false;
                }
                var categoryNew = new Categories
                {
                    CategoryName = categoriesDto.CategoryName,
                    CreatedDate = DateTime.Now,
                    Description = categoriesDto.Description,

                };
                _context.Categories.Add(categoryNew);
                var CreateResult = await _context.SaveChangesAsync();  
                return CreateResult > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }
        public async Task<List<Categories>> SearchCategoriesAsync(string searchTerm, int pageNumber, int pageSize)
        {
            return await _context.Categories
                                 .Where(c => c.CategoryName.ToLower().Contains(searchTerm.ToLower()))
                                 .Skip((pageNumber - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToListAsync();
        }


    }

}


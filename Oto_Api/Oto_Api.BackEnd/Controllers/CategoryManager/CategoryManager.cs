using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.CategoriesDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Oto_Api.BackEnd.Controllers.CategoryManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryManager : Controller
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryManager(ICategoryRepository categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        [HttpGet("GetAll_Categories")]
        public async Task<IActionResult> GetAllPaged(int pageNumber = 1, int pageSize = 3)
        {
            var data = await _categoryRepository.GetPagedAsync(pageNumber, pageSize);
            var total = await _categoryRepository.GetTotalCountAsync();

            return Ok(new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalRecords = total,
                totalPages = (int)Math.Ceiling((double)total / pageSize),
                data = data
            });
        }

        [HttpGet("CategoryById")]
        public async Task<IActionResult> CategoryById(int id)
        {
            var CateById = await _categoryRepository.GetCategoryByIdAsync(id);
            if (CateById == null) {
                return NotFound(new { message = "not found category" });
            }

            return Ok(new {message = $" This is category with id = {id}", category = CateById});
         }
        [HttpPut("Edit_Category/{id}")]
        public async Task<IActionResult> EditCategory( int id, [FromBody]  CategoriesDto categoriesDto)
        {

            var editCate = await _categoryRepository.GetCategoryByIdAsync(id);
            if (editCate == null)
            {
                return NotFound($"Category with id = {id} not found");
            }
            bool editResult = await _categoryRepository.EditCategoryAsync(id, categoriesDto);
            if (editResult)
            {
                return Ok("Category updated successfully.");
            }
            else
            {
                return BadRequest("Error updating category.");
            }
        }
        [HttpDelete("Delete_Category/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            bool deleteCate = await _categoryRepository.DeleteCategoryAsync(id);
            if (deleteCate)
            {
                return Ok(new { successfully = true, message = $"delete category with id = {id} successfully" });
            }
            else
            {
                return BadRequest(new { error = true, message = $"Error while deleting or not found Category with id = {id}" });
            }
        }
        [HttpPost("Create_Category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoriesDto categoriesDto)
        {
            var CreateCate = await _categoryRepository.CreateCategoryAsync(categoriesDto);
            if(!CreateCate)
            {
                return BadRequest(new { error = true, message = "Category already exists" });

            }
            else 
            {
                return Ok(new {successfully = true, message = "Create Category successfully!"});
            }
        }
        [HttpGet("Search")]
        public async Task<IActionResult> SearchCategories(string searchTerm = "", int pageNumber = 1, int pageSize = 3)
        {
            var categories = await _categoryRepository.SearchCategoriesAsync(searchTerm, pageNumber, pageSize);
            var totalCount = await _categoryRepository.CountCategoriesAsync(searchTerm);

            if (categories == null || !categories.Any())
            {
                return NotFound(new { message = "No categories found" });
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return Ok(new
            {
                data = categories,
                currentPage = pageNumber,
                
                pageSize = pageSize,
                totalRecords = totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
               
            });
        }



    }
}

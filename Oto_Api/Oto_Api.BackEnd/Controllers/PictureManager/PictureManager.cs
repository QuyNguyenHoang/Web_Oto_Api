using Microsoft.AspNetCore.Mvc;
using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Application.Interfaces;
using Oto_Api.Infrastructure.Price;

namespace Oto_Api.BackEnd.Controllers.PictureManager
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureManager : Controller
    {
        private readonly IPictureRepository _pictureRepository;
        public PictureManager(IPictureRepository pictureRepository)
        {
            _pictureRepository = pictureRepository;
        }
        [HttpGet("GetPictureById")]
        public async Task<IActionResult> GetPictureById(int id)
        {
            var pictureById = await _pictureRepository.GetPictureByIdAsync(id);
            if (pictureById == null)
            {
                return NotFound(new
                {
                    error = true,
                    message = $"Not found Picture with id = {id}"
                });
            }
            else
            {
                return Ok(new
                {
                    successfullt = true,
                    message = $"This is Picture with id= {id}",
                    data = pictureById,
                });
            }

        }
        [HttpGet("GetAll_Picture")]
        public async Task<IActionResult> GetAllPicture(int pageNumber = 1, int pageSize = 10)
        {
            var pictureData = await _pictureRepository.GetPicturePageAsync(pageNumber, pageSize);
            var pictureCount = await _pictureRepository.PictureCountAsync();
            return Ok(new
            {
                pageCurrent = pageNumber,
                pageSize = pageSize,
                totalRecords = pictureCount,
                totalPage = (int)Math.Ceiling((double)pictureCount / pageSize),
                data = pictureData
            });
        }
        [HttpPost("Create_Picture")]
        public async Task<IActionResult> CreatePicture(PictureDto pictureDto)
        {
            var createResult = await _pictureRepository.CreatePictureAsync(pictureDto);
            if (!createResult)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not create Picture"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Create is successfully!",
                    data = createResult,
                });
            }
        }
        [HttpPost("Update_Picture")]
        public async Task<IActionResult> UpdatePicture(int id, PictureDto pictureDto)
        {
            var updateResult =await _pictureRepository.UpdatePictureAsync(id, pictureDto);
            if (!updateResult)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Update Picture"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Update is successfully!",
                    data = updateResult,
                });
            }

        }
        [HttpDelete("Delete_Picture")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            var deleteResult = await _pictureRepository.DeletePictureAsync(id);
            if (!deleteResult)
            {
                return BadRequest(new
                {
                    error = true,
                    message = "Can not Delete Picture"
                });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = "Delete is successfully!",
                    data = deleteResult,
                });
            }
           
        }
        [HttpGet("Search_Picture")]
        public async Task<IActionResult> SearchPicture(string searchTerm, int pageNumber = 1,int pageSize = 10)
        {
            var searchResult = await _pictureRepository.SearchPictureAsync(searchTerm, pageNumber, pageSize);
            if (searchResult == null || !searchResult.Any())
            {
                return NotFound(new { message = " Not found Picture" });
            }
            else
            {
                return Ok(new
                {
                    successfully = true,
                    message = $"This is the result for the Picture searched with the keyword = {searchTerm}",
                    data = searchResult
                });
            }
        }
    }
}

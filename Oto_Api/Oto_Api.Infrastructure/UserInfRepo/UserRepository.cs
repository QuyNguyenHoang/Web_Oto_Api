using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.UserDTO;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data; 
using System.Threading.Tasks;

namespace Oto_Api.Infrastructure.UserInfRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> GetUserDtoByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(u => u.UserInfo)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FullName = user.UserInfo?.FullName,
                Address = user.UserInfo?.Address,
                BirthDate = user.UserInfo?.BirthDate,
                Picture = user.UserInfo?.Picture,
                Sex = user.UserInfo?.Sex,
                CreatedDate = user.UserInfo?.CreatedDate ?? DateTime.MinValue,
                Hobbies = user.UserInfo?.Hobbies,
                PhoneNumber = user.UserInfo?.PhoneNumber,
                Facebook = user.UserInfo?.Facebook,
                Website = user.UserInfo?.Website,
                Country = user.UserInfo?.Country,
                Province = user.UserInfo?.Province,
                City = user.UserInfo?.City,
                District = user.UserInfo?.District,
                Ward = user.UserInfo?.Ward
            };
        }
        public async Task<UserDto> CreateOrUpdateUserAsync(UserDto request)
        {

            var existingUser = await _context.UserInfos
        .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingUser != null)
            {
                // Cập nhật thông tin nếu đã tồn tại
                existingUser.FullName = request.FullName;
                existingUser.Address = request.Address;
                existingUser.BirthDate = request.BirthDate;
                existingUser.Picture = request.Picture;
                existingUser.Sex = request.Sex;
                existingUser.Hobbies = request.Hobbies;
                existingUser.PhoneNumber = request.PhoneNumber;
                existingUser.Facebook = request.Facebook;
                existingUser.Website = request.Website;
                existingUser.Country = request.Country;
                existingUser.Province = request.Province;
                existingUser.City = request.City;
                existingUser.District = request.District;
                existingUser.Ward = request.Ward;

               

                _context.UserInfos.Update(existingUser);
            }
            else
            {
                // Nếu chưa có thì thêm mới
                var newUserInfo = new UserInfo
                {
                    Id = request.Id,
                    FullName = request.FullName,
                    Address = request.Address,
                    BirthDate = request.BirthDate,
                    Picture = request.Picture,
                    Sex = request.Sex,
                    CreatedDate = DateTime.UtcNow,
                    Hobbies = request.Hobbies,
                    PhoneNumber = request.PhoneNumber,
                    Facebook = request.Facebook,
                    Website = request.Website,
                    Country = request.Country,
                    Province = request.Province,
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward
                };

                await _context.UserInfos.AddAsync(newUserInfo);
            }

            await _context.SaveChangesAsync();

            // Trả về DTO sau khi tạo/cập nhật
            var userDto = new UserDto
            {
                Id = request.Id,
                FullName = request.FullName,
                Address = request.Address,
                BirthDate = request.BirthDate,
                Picture = request.Picture,
                Sex = request.Sex,
                CreatedDate = existingUser?.CreatedDate ?? DateTime.UtcNow,
                Hobbies = request.Hobbies,
                PhoneNumber = request.PhoneNumber,
                Facebook = request.Facebook,
                Website = request.Website,
                Country = request.Country,
                Province = request.Province,
                City = request.City,
                District = request.District,
                Ward = request.Ward
            };

            return userDto;
        }



    }
}

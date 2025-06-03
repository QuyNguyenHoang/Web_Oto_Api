using Oto_Api.Application.DTOs.UserDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserDtoByIdAsync(string id);
        Task<UserDto> CreateOrUpdateUserAsync(UserDto request);
    }
}

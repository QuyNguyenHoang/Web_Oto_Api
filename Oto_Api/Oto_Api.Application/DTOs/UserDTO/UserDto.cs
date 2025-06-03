using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.UserDTO
{
    public class UserDto
    {

            public int UserInfoId { get; set; }
            public string Id { get; set; } = string.Empty;

            public string UserName { get; set; } = string.Empty;

            public string? Email { get; set; }

            // Thông tin từ UserInfo
            public string? FullName { get; set; }

            public string? Address { get; set; }

            public DateTime? BirthDate { get; set; }

            public string? Picture { get; set; }

            public bool? Sex { get; set; }

            public DateTime CreatedDate { get; set; }

            public string? Hobbies { get; set; }

            public string? PhoneNumber { get; set; }

            public string? Facebook { get; set; }

            public string? Website { get; set; }

            public string? Country { get; set; }

            public string? Province { get; set; }

            public string? City { get; set; }

            public string? District { get; set; }

            public string? Ward { get; set; }
        

    }
}

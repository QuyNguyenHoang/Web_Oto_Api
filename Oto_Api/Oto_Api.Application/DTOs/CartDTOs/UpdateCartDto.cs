using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.CartDTOs
{
    public class UpdateCartDto
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}

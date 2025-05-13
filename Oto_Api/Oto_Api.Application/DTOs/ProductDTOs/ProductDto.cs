using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public bool IsAvailable { get; set; } = true;
        public int CategoryId { get; set; }
    }
}

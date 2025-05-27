using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Application.DTOs.PriceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.ProductDTOs
{
    

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsAvailable { get; set; } = true;
        public int CategoryId { get; set; }

        // Thêm danh sách ảnh
        public List<PictureDto>? Pictures { get; set; }
        public decimal? Prices { get; set; }
    }
}

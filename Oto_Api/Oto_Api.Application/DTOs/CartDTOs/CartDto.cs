using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oto_Api.Application.DTOs.PictureDTOs;
using Oto_Api.Application.DTOs.ProductDTOs;

namespace Oto_Api.Application.DTOs.CartDTOs
{
    public class CartDto
    {
        [Key]
        public int CartId { get; set; }  // Khóa chính
        public string? UserId { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        public decimal? TotalPrice => (Price * Quantity) - (Discount ?? 0);

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public bool? Status { get; set; } 

        public decimal? Discount { get; set; }

        public string? SessionId { get; set; }

        public string? ShippingMethod { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public List<PictureDto>? Pictures { get; set; }
        public decimal? Prices { get; set; }

       public ProductDto? Products { get; set; }

    }
}

using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.CartDTOs
{
    public class CartDto
    {
        public string? Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        public decimal? TotalPrice => (Price * Quantity) - (Discount ?? 0);

        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime DateUpdated { get; set; } = DateTime.Now;

        public bool? Status { get; set; } 

        public decimal? Discount { get; set; }

        public string? SessionId { get; set; }

        public string? ShippingMethod { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; } //Ngay giao hang du kien

    }
}

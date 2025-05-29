using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.OderDTO
{
    public class OrderDto
    {
        [Key]
        public int OrderId { get; set; }
        public string? Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string? ShippingAddress { get; set; }
        public List<OrderDetailDto>? OrderDetails { get; set; }
    }
    public class OrderDetailDto
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

       
    }
}

using Microsoft.EntityFrameworkCore;
using Oto_Api.Application.DTOs.OderDTO;
using Oto_Api.Application.Interfaces;
using Oto_Api.Domain.Entities;
using Oto_Api.Infrastructure.Data;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateOrderAsync(OrderDto dto)
    {
        try
        {
            var order = new Orders
            {
                Id = dto.Id,
                TotalAmount = dto.TotalAmount,
                ShippingAddress = dto.ShippingAddress,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderDetails = dto.OrderDetails.Select(d => new OrderDetails
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi tạo đơn hàng:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException: {ex.InnerException.Message}");
            }
            return false;
        }
    }
    public async Task<List<Orders>> GetOrdersByUserIdAsync(string Id)
    {
        var listOrder = await _context.Orders
            .Where(o => o.Id == Id)
            .ToListAsync();

        return listOrder;
    }
    public async Task<OrderDto?> GetOrderWithDetailsAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null) return null;

        var orderDto = new OrderDto
        {
            Id = order.Id, // hoặc order.Id nếu Id là string
            TotalAmount = (decimal)order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
            {
                ProductId = od.ProductId,
                Quantity = od.Quantity,
                Price = od.Price
            }).ToList()
        };

        return orderDto;
    }
}


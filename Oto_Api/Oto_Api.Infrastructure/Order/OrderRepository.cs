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


    public async Task<int> CountOrderAsync(string searchTerm, bool? statusFilter = null)
    {
        var query = _context.Orders.Include(o => o.User).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();

            if (DateTime.TryParse(searchTerm, out DateTime searchDate))
            {
                query = query.Where(o => o.OrderDate.Date == searchDate.Date);
            }
            else
            {
                query = query.Where(o => o.User.UserName.ToLower().Contains(searchTerm)
                                      || o.User.Email.ToLower().Contains(searchTerm)
                                      || o.Status.ToString().ToLower().Contains(searchTerm));
            }
        }

        if (statusFilter.HasValue)
        {
            query = query.Where(o => o.Status == statusFilter.Value);
        }

        return await query.CountAsync();
    }

    public async Task<List<OrderDto>> SearchOrderAsync(string searchTerm = "", int pageSize = 10, int pageNumber = 1, bool? statusFilter = null)
    {
        var query = _context.Orders.Include(o => o.User).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.ToLower();

            if (DateTime.TryParse(searchTerm, out DateTime searchDate))
            {
                query = query.Where(o => o.OrderDate.Date == searchDate.Date);
            }
            else
            {
                query = query.Where(o => o.User.UserName.ToLower().Contains(searchTerm)
                                      || o.User.Email.ToLower().Contains(searchTerm)
                                      || o.Status.ToString().ToLower().Contains(searchTerm));
            }
        }

        if (statusFilter.HasValue)
        {
            query = query.Where(o => o.Status == statusFilter.Value);
        }

        return await query
            .OrderBy(o => o.Status)               // false lên trên, true xuống dưới
            .ThenByDescending(o => o.OrderDate)  // trong nhóm sắp xếp theo ngày mới nhất trước
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                UserName = o.User.UserName,
                TotalAmount = (decimal)o.TotalAmount,
                Status = o.Status
            })
            .ToListAsync();

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
                Status = true,
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
                .ThenInclude(od => od.Product) // Quan trọng: lấy thêm dữ liệu Product
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null) return null;

        var orderDto = new OrderDto
        {
            OrderId = order.OrderId,
            Id = order.Id,
            TotalAmount = order.TotalAmount ?? 0,
            ShippingAddress = order.ShippingAddress,
            OrderDate = order.OrderDate,
            Status = order.Status,

            OrderDetails = order.OrderDetails?.Select(od => new OrderDetailDto
            {
                ProductId = od.ProductId,
                ProductName = od.Product?.ProductName ?? "Không có tên", // An toàn tránh null
                Quantity = od.Quantity,
                Price = od.Price
            }).ToList()
        };

        return orderDto;
    }

    public async Task<bool> UpdateOrderAsync(OrderDto dto, int id)
    {
        try
        {
            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            if (existingOrder == null)
            {
                Console.WriteLine("Không tìm thấy đơn hàng cần cập nhật.");
                return false;
            }

            // Chỉ cập nhật trạng thái (Stust)
            existingOrder.Status = dto.Status;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi khi cập nhật đơn hàng:");
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine($"StackTrace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"InnerException: {ex.InnerException.Message}");
            }
            return false;
        }
    }
    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Where(o => o.OrderId == id)
            .Include(o=> o.User)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                Id = o.Id,
                TotalAmount = (decimal)o.TotalAmount,
                ShippingAddress = o.ShippingAddress,
                OrderDate = o.OrderDate,
                UserName = o.User.UserName,
                Status = o.Status
            })
            .FirstOrDefaultAsync();
    }
    public async Task<Orders?> DeleteOrderAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
            return null;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return order;
    }




}


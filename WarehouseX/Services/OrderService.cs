using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseX.Data;
using WarehouseX.DTOs;
using WarehouseX.Models;

namespace WarehouseX.Services
{
    public class OrderService : IOrderService
    {
        private readonly WarehouseDbContext _context;

        public OrderService(WarehouseDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync(int page, int pageSize)
        {
            // Optimized query: AsNoTracking, Pagination at DB level, Projection using Select (no unnecessary joins/columns)
            var orders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.OrderDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderDto> GetOrderAsync(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderId == id)
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == createOrderDto.CustomerId);
            if (!customerExists) throw new ArgumentException("Invalid CustomerId");

            var order = new Order
            {
                CustomerId = createOrderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;
            foreach (var itemDto in createOrderDto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null || product.StockQuantity < itemDto.Quantity)
                    throw new InvalidOperationException($"Invalid product or insufficient stock for ProductId: {itemDto.ProductId}");

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };
                
                order.OrderItems.Add(orderItem);
                totalAmount += orderItem.Quantity * orderItem.UnitPrice;
                
                // Update stock (basic simulation)
                product.StockQuantity -= itemDto.Quantity;
            }

            order.TotalAmount = totalAmount;

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<bool> UpdateOrderAsync(int id, Order orderUpdate)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.Status = orderUpdate.Status;
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderDto>> SearchOrdersAsync(string customerName)
        {
            return await _context.Orders
                .AsNoTracking()
                .Where(o => o.Customer.Name.Contains(customerName))
                .Select(o => new OrderDto
                {
                    OrderId = o.OrderId,
                    CustomerName = o.Customer.Name,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount
                })
                .Take(50) // limit results
                .ToListAsync();
        }
    }
}

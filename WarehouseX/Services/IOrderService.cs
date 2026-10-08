using System.Collections.Generic;
using System.Threading.Tasks;
using WarehouseX.DTOs;
using WarehouseX.Models;

namespace WarehouseX.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync(int page, int pageSize);
        Task<OrderDto> GetOrderAsync(int id);
        Task<Order> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<bool> UpdateOrderAsync(int id, Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<IEnumerable<OrderDto>> SearchOrdersAsync(string customerName);
    }
}

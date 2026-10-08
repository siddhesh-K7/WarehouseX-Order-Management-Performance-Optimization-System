using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WarehouseX.Data;
using WarehouseX.Models;
using WarehouseX.Services;
using Xunit;

namespace WarehouseX.Tests
{
    public class OrderPerformanceTests
    {
        private DbContextOptions<WarehouseDbContext> _options;

        public OrderPerformanceTests()
        {
            _options = new DbContextOptionsBuilder<WarehouseDbContext>()
                .UseInMemoryDatabase(databaseName: "TestWarehouseDb")
                .Options;
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsOptimizedResultWithPagination()
        {
            // Arrange
            using (var context = new WarehouseDbContext(_options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                var customer = new Customer { Name = "Test Customer", Email = "test@example.com" };
                context.Customers.Add(customer);

                for (int i = 0; i < 50; i++)
                {
                    context.Orders.Add(new Order
                    {
                        CustomerId = 1,
                        OrderDate = System.DateTime.UtcNow.AddDays(-i),
                        Status = "Completed",
                        TotalAmount = 100 + i
                    });
                }
                await context.SaveChangesAsync();
            }

            using (var context = new WarehouseDbContext(_options))
            {
                var service = new OrderService(context);

                // Act
                var orders = await service.GetOrdersAsync(1, 10);

                // Assert
                Assert.Equal(10, orders.Count());
            }
        }
    }
}

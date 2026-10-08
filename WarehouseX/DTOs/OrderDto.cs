using System;
using System.Collections.Generic;

namespace WarehouseX.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace WarehouseX.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        
        public ICollection<Order> Orders { get; set; }
    }
}

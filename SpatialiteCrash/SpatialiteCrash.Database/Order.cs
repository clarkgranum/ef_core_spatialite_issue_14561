using System;
using System.Collections.Generic;
using System.Text;

namespace SpatialiteCrash.Database
{
    public class Order
    {
        public Guid Id { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

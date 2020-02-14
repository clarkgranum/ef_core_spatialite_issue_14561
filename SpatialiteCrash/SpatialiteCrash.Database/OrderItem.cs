using System;
using System.Collections.Generic;
using System.Text;

namespace SpatialiteCrash.Database
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}

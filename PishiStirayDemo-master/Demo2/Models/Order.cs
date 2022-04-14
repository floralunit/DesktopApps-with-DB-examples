using System;
using System.Collections.Generic;

namespace Demo2.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = null!;
        public DateTime OrderDeliveryDate { get; set; }
        public string OrderPickupPoint { get; set; } = null!;
    }
}

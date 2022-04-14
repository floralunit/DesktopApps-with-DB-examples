using System;
using System.Collections.Generic;

#nullable disable

namespace AromatniyMir.Entities
{
    public partial class Order
    {
        public Order()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public DateTime OrderCreateDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public int OrderPickupPoint { get; set; }
        public int? OrderUserId { get; set; }
        public int OrderConfirmCode { get; set; }
        public int OrderStatus { get; set; }

        public virtual PickUpPoint OrderPickupPointNavigation { get; set; }
        public virtual Status OrderStatusNavigation { get; set; }
        public virtual User OrderUser { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}

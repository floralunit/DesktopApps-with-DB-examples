using System;
using System.Collections.Generic;

#nullable disable

namespace AromatniyMir.Entities
{
    public partial class PickUpPoint
    {
        public PickUpPoint()
        {
            Orders = new HashSet<Order>();
        }

        public int PickUpPointId { get; set; }
        public string PickUpPointAddress { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}

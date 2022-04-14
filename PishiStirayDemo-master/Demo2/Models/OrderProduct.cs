using System;
using System.Collections.Generic;

namespace Demo2.Models
{
    public partial class OrderProduct
    {
        public int OrderId { get; set; }
        public string ProductArticleNumber { get; set; } = null!;
    }
}

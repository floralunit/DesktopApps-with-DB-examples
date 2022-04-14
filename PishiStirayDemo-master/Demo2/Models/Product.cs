using System;
using System.Collections.Generic;

namespace Demo2.Models
{
    public partial class Product
    {
        public string ProductArticleNumber { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public int ProductUint { get; set; }
        public decimal ProductCost { get; set; }
        public int ProductMaxDiscount { get; set; }
        public int ProductManufacturer { get; set; }
        public int ProductSupplier { get; set; }
        public int ProductCategory { get; set; }
        public byte ProductDiscountAmount { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string ProductDescription { get; set; } = null!;
        public byte[]? ProductPhoto { get; set; }
        public string ProductStatus { get; set; } = null!;
    }
}

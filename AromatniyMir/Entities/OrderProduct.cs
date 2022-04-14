using System;
using System.Collections.Generic;

#nullable disable

namespace AromatniyMir.Entities
{
    public partial class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        public string ProductArticle { get; set; }
        public int ProductAmount { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product ProductArticleNavigation { get; set; }
    }
}

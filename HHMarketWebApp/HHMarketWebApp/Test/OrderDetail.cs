namespace HHMarketWebApp.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        public int OrderDetailsId { get; set; }

        public short Amount { get; set; }

        public decimal ExtendedPrice { get; set; }

        public int OrderId { get; set; }

        public int ProductDetailsId { get; set; }

        public virtual Order Order { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }
    }
}

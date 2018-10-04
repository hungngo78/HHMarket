namespace HHMarketWebApp.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CartDetail
    {
        [Key]
        public int CartDetailsId { get; set; }

        public short Amount { get; set; }

        public decimal ExtendedPrice { get; set; }

        public short Type { get; set; }

        public int ProductDetailsId { get; set; }

        public int CartId { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }
    }
}

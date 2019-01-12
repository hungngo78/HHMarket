namespace HHMarketWebApp.Models
{
    using System;
    public partial class ShoppingCart
    {
        public ShoppingCart()
        {
        }

        public int CartId { get; set; }
        public DateTime DateOpen { get; set; }
        public int? UserId { get; set; }
        public string CustomerName { get; set; }
        
        
        public System.Collections.Generic.List<CartDetailItem> listItemCart { get; set; }
    }
}

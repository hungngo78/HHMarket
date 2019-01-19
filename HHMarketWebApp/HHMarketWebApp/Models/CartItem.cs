using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHMarketWebApp.Models
{
    public class CartItem
    {
        public decimal Price { get; set; }
        public int ProductDetailsId { get; set; } 
        public int Amount { get; set; }
    }
}
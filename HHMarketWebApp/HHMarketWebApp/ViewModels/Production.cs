using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHMarketWebApp.ViewModels
{
    public class Production
    {
        public int ProductId { get; set; }

        public string ProductionName { get; set; }

        public string Description { get; set; }

        public decimal MinPrice { get; set; }

        public decimal MaxPrice { get; set; }

        public string Picture { get; set; }

        public string Color { get; set; }
    }
}
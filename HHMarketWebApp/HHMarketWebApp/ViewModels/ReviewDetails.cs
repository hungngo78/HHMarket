using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHMarketWebApp.ViewModels
{
    public class ReviewDetails
    {
        public Production product { get; set; }

        public List<ReviewProduction> reviewList { get; set; }

        public Rating rating { get; set; }  
    }
}
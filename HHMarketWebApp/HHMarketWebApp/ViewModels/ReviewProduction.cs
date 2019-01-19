using System.Collections;

namespace HHMarketWebApp.ViewModels
{
    using System;
    
   
    public partial class ReviewProduction
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public short OverallRating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Count { get; set; }
        public string UserName { get; set; }
    }
}

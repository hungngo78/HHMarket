using System.Collections;

namespace HHMarketWebApp.Models
{
    using System;
    
   
    public partial class ReviewProduction
    {
        public int ReviewId { get; set; }

       
        public string Content { get; set; }

        public short OverallRating { get; set; }

        public DateTime ReviewDate { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
        
    }
}

namespace HHMarketWebApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;


    public  class SearchProduction
    {
        public SearchProduction()
        {
        }

        public string Picture { get; set; }
        
        public int ProductId { get; set; }
      
        public string Name { get; set; }
        
        public string Description { get; set; }

        public string CategoryName { get; set; }

        public string CategoryDescription { get; set; }

        public string Color { get; set; }

        public decimal Price { get; set; }

        public short OverallRating { get; set; }

        public int Count { get; set; }
        public System.Collections.Generic.List<SearchProduction> searchProductionList { get; set; }
       
    }
}

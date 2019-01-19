using System.Collections;

namespace HHMarketWebApp.ViewModels
{


    public class ProductionDetail
    {

        public int ProductDetailsId { get; set; }


        public string Color { get; set; }


        public string Size { get; set; }


        public string Picture { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public int ProductId { get; set; }

        public string Description { get; set; }

        public string ProductionName { get; set; }

        public System.Collections.Generic.List<ReviewProduction> reviewListData { get; set; }
        public System.Collections.Generic.List<ProductionDetail> listdata {get;set;}




    }
}

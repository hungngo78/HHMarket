namespace HHMarketWebApp.ViewModels
{

    public class CartDetailItem
    {
        public int CartDetailsId { get; set; }

        public short Amount { get; set; }

        public int CartId { get; set; }

        public short Type { get; set; }

        public decimal ExtendedPrice { get; set; }

        public int ProductDetailsId { get; set; }

        public decimal Price { get; set; }

        public string Picture { get; set; }

        public string ProductName { get; set; }

        public int ProductID { get; set; }

        public string Color { get; set; }

        public int UserId { get; set; }

        public int TotalAmountProduction { get; set; }

        //public string Username { get; set; }
        //public string CustomerName { get; set; }
    }
}

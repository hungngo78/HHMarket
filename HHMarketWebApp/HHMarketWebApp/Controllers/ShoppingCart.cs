using System.Data;
using System.Linq;
using System.Web.Mvc;
using HHMarketWebApp.Models;
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        public ActionResult Index(int userId)
        {
            DBModelContainer db = new DBModelContainer();

            ShoppingCart shoppingCart = new ShoppingCart();
            shoppingCart.listItemCart = (from c in db.Carts
                                         join cdetail in db.CartDetails on c.CartId equals cdetail.CartId
                                         join pdetail in db.ProductDetails on cdetail.ProductDetailsId equals pdetail.ProductDetailsId
                                         join p in db.Products on pdetail.ProductId equals p.ProductId
                                         where c.UserId == userId && cdetail.Type == 0
                                         select new CartDetailItem()
                                         {
                                             Amount = cdetail.Amount,
                                             CartId = cdetail.CartId,
                                             Type = cdetail.Type,
                                             CartDetailsId = cdetail.CartDetailsId,
                                             ExtendedPrice = cdetail.ExtendedPrice,
                                             ProductDetailsId = cdetail.ProductDetailsId,
                                             Price = pdetail.Price,
                                             Picture = pdetail.Picture,
                                             ProductName = p.Name,
                                             ProductID = pdetail.ProductId,
                                             Color = pdetail.Color,
                                             TotalAmountProduction = pdetail.Amount
                                         }).ToList();

            

           

            return View(shoppingCart);
        }

    }
}

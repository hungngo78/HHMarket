using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FormsAuth;
using HHMarketWebApp.Models;

namespace HHMarketWebApp.Controllers
{
    public class MyLayoutPageController : Controller
    {
        // GET: MySearch
        public ActionResult Index()
        {
            DBModelContainer db = new DBModelContainer();

            var categories = from c in db.Categories
                             select c;
            categories = categories.OrderBy(c => c.CategoryId).Take(20);

            ViewBag.categories = categories;

            return PartialView();
        }

        public ActionResult CartIcon()
        {
            int cartItemNumber = 0;

            if (FormsAuth.UserManager.User != null)
            {
                DBModelContainer db = new DBModelContainer();
                List<CartItem> cartItemList = (from c in db.Carts
                                               join cd in db.CartDetails on c.CartId equals cd.CartId
                                               where c.UserId == UserManager.User.Id
                                               select new CartItem
                                               {
                                                   ProductDetailsId = cd.ProductDetailsId,
                                                   Price = cd.ExtendedPrice,
                                                   Amount = cd.Amount
                                               }).ToList();

                cartItemNumber = cartItemList.Count();
            }
            else
            {
                HttpCookie reqIDListCookies = Request.Cookies["ProductDetailIDlist"];
                if (reqIDListCookies != null)
                {
                    string dataAsString = reqIDListCookies.Value;
                    if (!dataAsString.Equals(string.Empty))
                    {
                        List<int> listdata = new List<int>();
                        listdata = dataAsString.Split(',').Select(x => Convert.ToInt32(x)).ToList();

                        cartItemNumber = listdata.Count();
                    }
                }
            }

            ViewBag.cartItemNumber = cartItemNumber;
            return PartialView();
        }
    }
}
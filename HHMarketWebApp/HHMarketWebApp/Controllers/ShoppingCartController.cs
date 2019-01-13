using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HHMarketWebApp.Models;
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBModelContainer db = new DBModelContainer();
        public ActionResult Index(int userId)
        {
            
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
                                             UserId = c.UserId,
                                             TotalAmountProduction = pdetail.Amount
                                         }).ToList();

            return View(shoppingCart);
        }
        [HttpPost]
        public async Task<ActionResult> Order(CartDetailItem model)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.DeliveryDate = System.DateTime.Now;
                order.Status = 0;
                order.OrderDate = System.DateTime.Now;
                order.Note = "Note of user";
                order.UserId = model.UserId;
                db.Orders.Add(order);
                //await db.SaveChangesAsync();

                System.Collections.Generic.List<CartDetailItem> list = new System.Collections.Generic.List<CartDetailItem>();
                list = (from c in db.CartDetails
                        where c.CartId == model.CartId
                        select new CartDetailItem()
                        {
                            Amount = c.Amount,
                            ExtendedPrice = c.ExtendedPrice,
                            ProductDetailsId = c.ProductDetailsId,
                            CartDetailsId = c.CartDetailsId,
                            CartId = c.CartId
                         }).ToList();

                for (int i = 0; i < list.Count(); i++)
                {
                    OrderDetail orderDetail = new OrderDetail();
                    orderDetail.Amount = list[i].Amount;
                    orderDetail.ExtendedPrice = list[i].ExtendedPrice;
                    orderDetail.ProductDetailsId = list[i].ProductDetailsId;
                    orderDetail.OrderId = order.OrderId;
                    db.OrderDetails.Add(orderDetail);
                    
                    //var id = list[i].ProductDetailsId;
                    // var productionDetail = db.ProductDetails.Single(item => item.ProductDetailsId == id);
                    // reduce count in ProductDetail
                    //ProductDetail productionDetail = db.ProductDetails.FirstOrDefault(item => item.ProductDetailsId == id);
                    //productionDetail.Amount = (short)(productionDetail.Amount - list[i].Amount);
                }
                //await db.SaveChangesAsync();

                // delete cart and cartDetail
                //var x = db.CartDetails.Where(item => item.CartId == model.CartId);
                //db.CartDetails.RemoveRange(db.CartDetails.Where(c => c.CartId == model.CartId));
                db.CartDetails.Where(p => p.CartId == model.CartId)
                        .ToList().ForEach(p => db.CartDetails.Remove(p));

                //var x1 = db.Carts.FirstOrDefault(item => item.CartId == model.CartId);
                //db.Carts.Remove(x1);
                db.Carts.Where(p => p.CartId == model.CartId)
                        .ToList().ForEach(p => db.Carts.Remove(p));
                await db.SaveChangesAsync();
                
                ModelState.Clear();

                return this.Json(new
                {
                    EnableSuccess = true,
                    SuccessTitle = "Successful!",
                    SuccessMsg = "Thank you so much for your order!"
                });

            }

            return this.Json(new
            {
                EnableError = true,
                ErrorTitle = "Error",
                ErrorMsg = "Something goes wrong, please try again later"
            });

        }

    }
}


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FormsAuth;
using HHMarketWebApp.Models;
using HHMarketWebApp.ViewModels;
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBModelContainer db = new DBModelContainer();

        // Get
        public ActionResult Index()
        {
            ShoppingCart shoppingCart = new ShoppingCart();

            if (FormsAuth.UserManager.User != null)
            {
                int userId = UserManager.User.Id;
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
            }
            else  // get from Cookie
            {
                HttpCookie reqCookies;
                HttpCookie reqIDListCookies = Request.Cookies["ProductDetailIDlist"];
                if (reqIDListCookies != null)
                {                   
                    string dataAsString = reqIDListCookies.Value;
                    if (!dataAsString.Equals(string.Empty))
                    {
                        List<int> listdata = new List<int>();
                        List<CartItem> listCartItem = new List<CartItem>();

                        listdata = dataAsString.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        for (int i = 0; i < listdata.Count(); i++)
                        {
                            CartItem item = new CartItem();
                            item.ProductDetailsId = listdata[i];

                            reqCookies = Request.Cookies["CartItems[" + item.ProductDetailsId.ToString() + "]"];
                            if (reqCookies != null)
                            {
                                CartItem cookiesItem = new JavaScriptSerializer().Deserialize<CartItem>(reqCookies.Value);

                                item.Amount = cookiesItem.Amount;
                                item.Price = cookiesItem.Price;

                                listCartItem.Add(item);
                            }
                        }

                        // 
                        if (listCartItem.Count() > 0)
                        {
                            shoppingCart.listItemCart = (from l in listCartItem
                                                         join pd in db.ProductDetails on l.ProductDetailsId equals pd.ProductDetailsId
                                                         join p in db.Products on pd.ProductId equals p.ProductId
                                                         where listdata.Any(w => l.ProductDetailsId.Equals(w))
                                                         select new CartDetailItem()
                                                         {
                                                             Amount = (short)l.Amount,
                                                             CartId = 0,
                                                             Type = 0,
                                                             CartDetailsId = 0,
                                                             ExtendedPrice = l.Price,
                                                             ProductDetailsId = l.ProductDetailsId,
                                                             Price = pd.Price,
                                                             Picture = pd.Picture,
                                                             ProductName = p.Name,
                                                             ProductID = pd.ProductId,
                                                             Color = pd.Color,
                                                             UserId = 0,
                                                             TotalAmountProduction = pd.Amount
                                                         }).ToList();
                        }
                    }
                }
            }

            return View(shoppingCart);
        }

        [HttpPost]
        public async Task<ActionResult> AddToCart(CartItem item)
        {
            if (ModelState.IsValid)
            {
                Cart cart = null;
                CartDetail detail = null;

                if (UserManager.User != null)
                {
                    // get existing Cart
                    cart = (from c in db.Carts
                                   where c.UserId == UserManager.User.Id
                                   select c).FirstOrDefault();

                    // there is no existing Shopping Cart for this user -> create new Shopping Cart
                    if (cart == null)
                    {
                        // create new Shopping Cart
                        cart = new Cart();
                        cart.DateOpen = System.DateTime.Now;
                        cart.UserId = UserManager.User.Id;
                        db.Carts.Add(cart);

                        // save Cart Item
                        detail = new CartDetail();
                        detail.Amount = item.Amount;
                        db.CartDetails.Add(detail);
                    }
                    else  // there is already an existing Shopping Cart for this user
                    {
                        // get Cart Item for this ProductDetailId
                        detail = (from cd in db.CartDetails
                                  where cd.CartId == cart.CartId && cd.ProductDetailsId == item.ProductDetailsId
                                  select cd).FirstOrDefault();
                        if (detail == null)
                        {
                            detail = new CartDetail();
                            detail.Amount = item.Amount;
                            db.CartDetails.Add(detail);
                        }
                        else
                        {
                            detail.Amount = (short)(detail.Amount + item.Amount);
                        }
                    }

                    detail.ExtendedPrice = item.Price;
                    detail.ProductDetailsId = item.ProductDetailsId;
                    detail.CartId = cart.CartId;

                    await db.SaveChangesAsync();
                }
                else
                {
                    // get tmp Cart in cookie
                    string dateOfOpen = string.Empty;
                    HttpCookie reqCookies = Request.Cookies["CartInfo"];
                    if (reqCookies != null)  // there is a temporary Cart in cookies 
                    {
                        dateOfOpen = reqCookies["DateOfOpen"].ToString();

                        /* save Cart Item in cookie */
                        // read saved cart item for given ProductDetailId
                        HttpCookie reqCartItemCookies = Request.Cookies["CartItems["+ item.ProductDetailsId.ToString() + "]"];  // cartItem from request
                        HttpCookie respCartItemCookie;  // cartItem in response
                        if (reqCartItemCookies != null)
                        {
                            CartItem mItem = new JavaScriptSerializer().Deserialize<CartItem>(reqCartItemCookies.Value);
                            mItem.Amount = (short)(mItem.Amount + item.Amount);
                            string myObjectJson = new JavaScriptSerializer().Serialize(mItem);
                            respCartItemCookie = new HttpCookie("CartItems[" + item.ProductDetailsId.ToString() + "]", myObjectJson)
                            {
                                Expires = DateTime.Now.AddDays(1)
                            };
                        }
                        else
                        {
                            item.Amount = 1;
                            string myObjectJson = new JavaScriptSerializer().Serialize(item);
                            respCartItemCookie = new HttpCookie("CartItems[" + item.ProductDetailsId + "]", myObjectJson)
                            {
                                Expires = DateTime.Now.AddDays(1)
                            };
                        }
                        HttpContext.Response.Cookies.Add(respCartItemCookie);

                        /* add productDetailID in cookies */
                        HttpCookie reqIDListCookies = Request.Cookies["ProductDetailIDlist"];
                        if (reqIDListCookies != null)
                        {
                            string yourListString = string.Empty;
                            string dataAsString = reqIDListCookies.Value;
                            List<int> listdata = new List<int>();
                            if (!dataAsString.Equals(string.Empty))
                            {
                                listdata = dataAsString.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                            }
                            if (!listdata.Contains(item.ProductDetailsId))
                            {
                                listdata.Add(item.ProductDetailsId);
                            }
                            // Stringify your list
                            yourListString = String.Join(",", listdata);
                            HttpCookie IDListCookies = new HttpCookie("ProductDetailIDlist", yourListString)
                            {
                                Expires = DateTime.Now.AddDays(1)
                            };
                            HttpContext.Response.Cookies.Add(IDListCookies);
                        }
                        else
                        {
                            List<int> listdata = new List<int>();
                            listdata.Add(item.ProductDetailsId);

                            // Stringify your list
                            var yourListString = String.Join(",", listdata);
                            HttpCookie IDListCookies = new HttpCookie("ProductDetailIDlist", yourListString)
                            {
                                Expires = DateTime.Now.AddDays(1)
                            };
                            HttpContext.Response.Cookies.Add(IDListCookies);
                        }
                    }
                    else  // There is not any tmp Shopping Cart for this user 
                    {
                        // create new Shopping Cart in Cookie                   
                        HttpCookie cartCookie = new HttpCookie("CartInfo")
                        {
                            Expires = DateTime.Now.AddYears(1)
                        };
                        cartCookie["DateOfOpen"] = DateTime.Now.ToString();
                        HttpContext.Response.Cookies.Add(cartCookie);

                        // save Cart Item in cookie
                        item.Amount = 1;
                        string myObjectJson = new JavaScriptSerializer().Serialize(item);
                        HttpCookie cartItemCookie = new HttpCookie("CartItems[" + item.ProductDetailsId + "]", myObjectJson)
                        {
                            Expires = DateTime.Now.AddDays(1)
                        };
                        HttpContext.Response.Cookies.Add(cartItemCookie);

                        /* add productDetailID into cookies */
                        List<int> listdata = new List<int>();
                        listdata.Add(item.ProductDetailsId);
                        // Stringify your list
                        var yourListString = String.Join(",", listdata);
                        HttpCookie IDListCookies = new HttpCookie("ProductDetailIDlist", yourListString)
                        {
                            Expires = DateTime.Now.AddDays(1)
                        };
                        HttpContext.Response.Cookies.Add(IDListCookies);
                    }
                }
                    
                ModelState.Clear();
                return this.Json(new
                {
                    EnableSuccess = true,
                    SuccessTitle = "Successful!",
                    SuccessMsg = "Add cart successfully order!"
                });
            }

            return this.Json(new
            {
                EnableError = true,
                ErrorTitle = "Error",
                ErrorMsg = "Something goes wrong, please try again later"
            });

        }

        [HttpPost]
        public async Task<ActionResult> UpdateQuantity(CartDetailItem model)
        {
            if (ModelState.IsValid)
            {
                // update amount to cart detail table'
                if (model.Amount == 0)
                {
                    db.CartDetails.Where(p => p.CartDetailsId == model.CartDetailsId)
                        .ToList().ForEach(p => db.CartDetails.Remove(p));
                    await db.SaveChangesAsync();


                    var list = db.CartDetails.Where(p => p.CartId == model.CartId).ToList();
                    if (list.Count() <= 0)
                    {
                        db.Carts.Where(p => p.CartId == model.CartId)
                          .ToList().ForEach(p => db.Carts.Remove(p));
                    }

                    await db.SaveChangesAsync();
                }
                else
                {
                    CartDetail cartDetail = db.CartDetails.FirstOrDefault(item => item.CartDetailsId == model.CartDetailsId);
                    cartDetail.Amount = (short)(model.Amount);
                    await db.SaveChangesAsync();
                }
                ModelState.Clear();

                return this.Json(new
                {
                    ResponseType = Config.SUCCESS,
                    Msg = "Update quantity successfully!"
                  
                });
            }

            return this.Json(new
            {
                ResponseType = Config.SUCCESS,
                Msg = "Something goes wrong, please try again later"
            });
        }        

        [HttpPost]
        public async Task<ActionResult> RemoveCartItem(CartDetailItem model)
        {
            if (ModelState.IsValid)
            {
                if (FormsAuth.UserManager.User != null)
                {
                    // update amount to cart detail table
                    db.CartDetails.Where(p => p.CartDetailsId == model.CartDetailsId)
                         .ToList().ForEach(p => db.CartDetails.Remove(p));
                    await db.SaveChangesAsync();

                    
                    var list = db.CartDetails.Where(p => p.CartId == model.CartId).ToList();
                    if (list.Count() <= 0)
                    {
                        db.Carts.Where(p => p.CartId == model.CartId)
                          .ToList().ForEach(p => db.Carts.Remove(p));
                    }

                    await db.SaveChangesAsync();
                }
                else
                {
                    /* remove cart item of this ProductDetailId in cookies */
                    HttpCookie reqCartItemCookies = Request.Cookies["CartItems[" + model.ProductDetailsId.ToString() + "]"]; 
                    if (reqCartItemCookies != null)
                    {
                        var c = new HttpCookie("CartItems[" + model.ProductDetailsId.ToString() + "]");
                        c.Expires = DateTime.Now.AddDays(-1D);
                        Response.Cookies.Add(c);     
                    }

                    /* update productDetailID list in cookies */
                    HttpCookie reqIDListCookies = Request.Cookies["ProductDetailIDlist"];
                    if (reqIDListCookies != null)
                    {
                        string dataAsString = reqIDListCookies.Value;
                        List<int> listdata = new List<int>();
                        listdata = dataAsString.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                        listdata.Remove(model.ProductDetailsId);

                        // Stringify your list
                        var yourListString = String.Join(",", listdata);

                        HttpCookie IDListCookies = new HttpCookie("ProductDetailIDlist", yourListString)
                        {
                            Expires = DateTime.Now.AddDays(1)
                        };
                        HttpContext.Response.Cookies.Add(IDListCookies);
                    }
                }

                ModelState.Clear();
                return this.Json(new
                {
                    ResponseType = Config.SUCCESS,
                    Msg = "Thank you so much for your order!"
                });

            }

            return this.Json(new
            {
                ResponseType = Config.SUCCESS,
                Msg = "Something goes wrong, please try again later"
            });
        }

        [HttpPost]
        public async Task<ActionResult> Order(CartDetailItem model)
        {
            if (UserManager.User != null)
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
                        ProductDetail productionDetail = db.ProductDetails.FirstOrDefault(item => item.ProductDetailsId == model.ProductDetailsId);
                        productionDetail.Amount = (short)(productionDetail.Amount - list[i].Amount);
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
                        ResponseType = Config.SUCCESS,
                        Msg = "Thank you so much for your order!"
                    });
                }

                return this.Json(new
                {
                    ResponseType = Config.SOMETHING_WRONG_WITH_POST_REQUEST,
                    Msg = "Something goes wrong, please try again later"
                });
            }

            return this.Json(new
            {
                ResponseType = Config.NEED_LOGIN,
                Msg = "Please login first"
            });
        }
    }
}


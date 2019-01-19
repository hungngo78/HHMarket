using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HHMarketWebApp.Models;
using FormsAuth;
using System.Web.Script.Serialization;

namespace HHMarketWebApp.Controllers
{
    public class UserController : Controller
    {
        private DBModelContainer db = new DBModelContainer();

        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        // POST 
        [HttpPost]
        //public ActionResult Login(String username, String password)
        public async Task<ActionResult> Login([Bind(Include = "Username,Password")]Logon logon)
        {
            if (ModelState.IsValid)
            {
                // Authenticate the user.
                if (UserManager.ValidateUser(logon, Response))
                {
                    /* move temporary shopping cart to DB */
                    string dateOfOpen = string.Empty;

                    // get tmp Cart in cookie
                    HttpCookie reqCartInfoCookies = HttpContext.Request.Cookies["CartInfo"];
                    if (reqCartInfoCookies != null)  // there is a temporary Cart in cookies 
                    {
                        dateOfOpen = reqCartInfoCookies["DateOfOpen"].ToString();

                        Cart cart = null;
                        // get existing Cart
                        cart = (from c in db.Carts
                                where c.UserId == UserManager.User.Id
                                select c).FirstOrDefault();

                        // there is no existing Shopping Cart for this user -> create new Shopping Cart
                        if (cart == null)
                        {
                            cart = new Cart();
                            cart.UserId = UserManager.User.Id;
                            cart.DateOpen = DateTime.Parse(dateOfOpen);
                            db.Carts.Add(cart);
                        }

                        HttpCookie reqIDListCookies = Request.Cookies["ProductDetailIDlist"];
                        if (reqIDListCookies != null)
                        {
                            string dataAsString = reqIDListCookies.Value;
                            if (!dataAsString.Equals(string.Empty))
                            {
                                List<int> listdata = new List<int>();
                                //List<CartItem> listCartItem = new List<CartItem>();
                                listdata = dataAsString.Split(',').Select(x => Int32.Parse(x)).ToList();
                                for (int i = 0; i < listdata.Count(); i++)
                                {
                                    HttpCookie reqCartItemCookies = Request.Cookies["CartItems[" + listdata[i].ToString() + "]"];
                                    if (reqCartItemCookies != null)
                                    {
                                        CartItem cookiesItem = new JavaScriptSerializer().Deserialize<CartItem>(reqCartItemCookies.Value);

                                        // get Cart Item for this ProductDetailId in DB
                                        CartDetail detail;
                                        int productDetailId = listdata[i];
                                        detail = (from cd in db.CartDetails
                                                  where cd.CartId == cart.CartId && cd.ProductDetailsId == productDetailId
                                                  select cd).FirstOrDefault();
                                        if (detail == null)
                                        {
                                            detail = new CartDetail();
                                            detail.Amount = (short)cookiesItem.Amount;
                                            detail.ExtendedPrice = cookiesItem.Price;
                                            detail.Type = 0;
                                            detail.ProductDetailsId = listdata[i];
                                            detail.CartId = cart.CartId;
                                            db.CartDetails.Add(detail);
                                        }
                                        else
                                        {
                                            detail.Amount += (short)cookiesItem.Amount;
                                        }

                                        await db.SaveChangesAsync();

                                        /* remove cart item of this ProductDetailId in cookies */                                        
                                        var respCartItemscookies = new HttpCookie("CartItems[" + listdata[i].ToString() + "]");
                                        respCartItemscookies.Expires = DateTime.Now.AddDays(-1D);
                                        Response.Cookies.Add(respCartItemscookies);
                                    }
                                }

                                /* update productDetailID list in cookies */
                                HttpCookie respIDListCookies = new HttpCookie("ProductDetailIDlist", "")
                                {
                                    Expires = DateTime.Now.AddDays(1)
                                };
                                HttpContext.Response.Cookies.Add(respIDListCookies);
                            }
                        }
                    }

                    // Redirect to the secure area.
                    return RedirectToAction("Index", "Category");
                }
            }

            ViewBag.LoginFailure = 1;
            return View();
        }

        // GET
        public ActionResult Logout()
        {
            // Clear the user session and forms auth ticket.
            UserManager.Logoff(Session, Response);

            return RedirectToAction("Login", "User");
        }

        #region Not Used
        // GET: User
        public async Task<ActionResult> Index()
        {
            return View(await db.Users.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,UserName,Password,Email,FirstName,LastName,Address,City,State,ZipCode")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserId,UserName,Password,Email,FirstName,LastName,Address,City,State,ZipCode")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        #endregion
    }
}

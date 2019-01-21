
using System.Web.Mvc;
using HHMarketWebApp.Models;
using FormsAuth;
using System.Web;
using System.Linq;

namespace HHMarketWebApp.Controllers
{
    public class RegistrationController : Controller
    {
        private DBModelContainer db = new DBModelContainer();

        //GET
        public ActionResult Index()
        {
             return View();
        }

        //GET
        public ActionResult Edit()
        {
            if (UserManager.User != null)
            {
                if (ModelState.IsValid)
                {
                    var user = (from u in db.Users
                                where u.UserId == UserManager.User.Id
                                select u
                                ).FirstOrDefault();

                    return View("Index", user);
                }
            }

            return RedirectToAction("Index", "Category");
        }

        // POST 
        [HttpPost]       
        public ActionResult Update([Bind(Include = "UserName,Password,Email, City, FirstName, LastName, State,Zipcode, Address")] User _user)
        {
            if (ModelState.IsValid)
            {
                if (UserManager.User != null)
                {
                    User user = (from u in db.Users
                                  where u.UserId == UserManager.User.Id
                                  select u
                                ).FirstOrDefault();
                    user.UserName   = _user.UserName;
                    user.Password   = _user.Password;
                    user.Email      = _user.Email;
                    user.City       = _user.City;
                    user.FirstName  = _user.FirstName;
                    user.LastName   = _user.LastName;
                    user.State      = _user.State;
                    user.ZipCode    = _user.ZipCode;
                    user.Address    = _user.Address;

                    db.SaveChanges();

                    ModelState.Clear();

                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    db.Users.Add(_user);
                    db.SaveChanges();
                
                    Logon logon = new Logon();
                    logon.Username = _user.UserName;
                    logon.Password = _user.Password;
                    if (_user.UserId > 0 && UserManager.ValidateUser(logon, Response))
                    {
                        ModelState.Clear();

                        return RedirectToAction("Index", "Category");
                    }
                }

                ModelState.Clear();
            }

            ViewBag.RegisterFailure = 1;    // errors ocured 
            return View("Index", _user);
        }

        
    }
}

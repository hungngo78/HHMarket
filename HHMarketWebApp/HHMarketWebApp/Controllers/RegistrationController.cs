
using System.Web.Mvc;
using HHMarketWebApp.Models;
using FormsAuth;

namespace HHMarketWebApp.Controllers
{
    public class RegistrationController : Controller
    {
       private DBModelContainer db = new DBModelContainer();

        //GET
        public ActionResult Index(User user)
        {
            return View(user);
        }

        // POST 
        [HttpPost]
        public ActionResult RegisterUser([Bind(Include = "UserName,Password,Email, City, FirstName, LastName, State,Zipcode, Address")]User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                Logon logon = new Logon();
                logon.Username = user.UserName;
                logon.Password = user.Password;
                if (user.UserId > 0 && UserManager.ValidateUser(logon, Response))
                {
                    ViewBag.LoginFailure = 0;
                    ModelState.Clear();
                    return RedirectToAction("Index", "Category");

                }
                ModelState.Clear();
              
            }
            ViewBag.LoginFailure = 1;
            return RedirectToAction("Index", "Registration", user);

        }
    }
}

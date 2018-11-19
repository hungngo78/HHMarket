
using System.Web.Mvc;
using HHMarketWebApp.Models;
using FormsAuth;
using System.Web;

namespace HHMarketWebApp.Controllers
{
    public class RegistrationController : Controller
    {
       private DBModelContainer db = new DBModelContainer();

        //GET
      //  public ActionResult Index(User user, int? error)
     //   {
       //     return View(user);
//}

        // POST 
       // [HttpPost]
       // [HttpGet]
        public ActionResult Index([Bind(Include = "UserName,Password,Email, City, FirstName, LastName, State,Zipcode, Address")]User user, int? error)
        {
            ViewBag.LoginFailure = error;
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
            if (ViewBag.LoginFailure == null)
            {
                ViewBag.LoginFailure = 2;
            }
            else
            {
                ViewBag.LoginFailure = 1;
            }
            //return RedirectToAction("Index", "Registration", user);
            return View(user);

        }
    }
}

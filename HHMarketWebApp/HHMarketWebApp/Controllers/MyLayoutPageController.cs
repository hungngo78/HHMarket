using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
    }
}
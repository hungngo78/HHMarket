using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HHMarketWebApp.Models;
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index(int? page)
        {
            DBModelContainer db = new DBModelContainer();
            //IQueryable<Category> categories = db.Category
            //                    .OrderBy(c => c.CategoryId);
            //.Skip(8 * (page - 1))
            //.Take(8);

            var categories = from c in db.Categories
                             select c;
            categories = categories.OrderBy(c => c.CategoryId);

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            //return View(categories.ToList());
            return View(categories.ToPagedList(pageNumber, pageSize));

        }
    }
}
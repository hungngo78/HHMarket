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

namespace HHMarketWebApp.Controllers
{
    public class SearchProductionController : Controller
    {

        public ActionResult Index(string searchText)
        {

            string[] searchArr = new string[] { "" };

            DBModelContainer db = new DBModelContainer();
            SearchProduction searchList = new SearchProduction();
            if (searchText != null && !searchText.Trim().Equals("")) //search all
            {
                searchText = searchText.ToLower().Trim();
                searchArr = searchText.Split(' ');
            }
            /*
from xx in table
where (from yy in string[]
       select yy).Contains(xx.uid.ToString())
select xx
*/

            searchList.searchProductionList = (from p in db.Products
                                               join category in db.Categories on p.CategoryId equals category.CategoryId
                                               join pdetail in db.ProductDetails on p.ProductId equals pdetail.ProductId
                                               //where (p.Name.Contains(searchText) || p.Description.Contains(searchText) || category.Name.Contains(searchText) || category.Description.Contains(searchText))
                                               // where (searchArr.Contains(p.Name) || searchArr.Contains(p.Description) || searchArr.Contains(category.Name) || searchArr.Contains(category.Description))

                                               //         where (from item in searchArr
                                               //               select item).Contains(p.Description.ToLower())//, p.Description)

                                               select new SearchProduction() {
                                                   Name = p.Name, Description = p.Description,
                                                   CategoryName = category.Name,
                                                   ProductId = p.ProductId,
                                                   Picture = pdetail.Picture,
                                                   Color = pdetail.Color,
                                                   Price = pdetail.Price,
                                                   CategoryDescription = category.Description }).ToList();
            //    searchList.searchProductionList = searchList.searchProductionList.GroupBy(c => c.ProductId);

            var s =  searchList.searchProductionList.Where(t => searchArr.Any(w => t.Description.ToLower().Contains(w)|| t.Name.ToLower().Contains(w) || t.CategoryName.ToLower().Contains(w) || t.CategoryName.ToLower().Contains(w))).ToList() ;
            searchList.searchProductionList = s;
            var s1 = searchList.searchProductionList.GroupBy(item => item.ProductId).Select(g => new SearchProduction()
            {
                ProductId = g.Key,
                searchProductionList = g.ToList()
           }).ToList();
            List<SearchProduction> list = s1;

            return View(list);
        }


    }}

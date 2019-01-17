
using System.Data;
using System.Linq;
using System.Web.Mvc;
using HHMarketWebApp.Models;
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class SearchProductionController : Controller
    {

        public ActionResult Index(string searchText)
        {

            string[] searchArr = new string[] { " " };

            DBModelContainer db = new DBModelContainer();
            SearchProduction searchList = new SearchProduction();
            if (searchText != null && !searchText.Trim().Equals("")) //search all
            {
                searchText = searchText.ToLower().Trim();
                searchArr = searchText.Split(' ');
            }

            searchList.searchProductionList = (from p in db.Products
                                               join category in db.Categories on p.CategoryId equals category.CategoryId
                                               join pdetail in db.ProductDetails on p.ProductId equals pdetail.ProductId
                                               //where (p.Name.Contains(searchText) || p.Description.Contains(searchText) || category.Name.Contains(searchText) || category.Description.Contains(searchText))
                                               // where (searchArr.Contains(p.Name) || searchArr.Contains(p.Description) || searchArr.Contains(category.Name) || searchArr.Contains(category.Description))

                                               //         where (from item in searchArr
                                               //               select item).Contains(p.Description.ToLower())//, p.Description)
                                               where searchArr.Any(w => p.Description.ToLower().Contains(w) || p.Name.ToLower().Contains(w) || category.Name.ToLower().Contains(w) || category.Description.ToLower().Contains(w))
                                               select new 
                                               {
                                                   Name = p.Name,
                                                   Description = p.Description,
                                                   CategoryName = category.Name,
                                                   ProductId = p.ProductId,
                                                   Picture = pdetail.Picture,
                                                   Color = pdetail.Color,
                                                   Price = pdetail.Price,
                                                   CategoryDescription = category.Description
                                               } into t1
                                               group t1 by t1.ProductId into g
                                               select new SearchProduction()
                                               {
                                                   Name = g.FirstOrDefault().Name,
                                                   Description = g.FirstOrDefault().Description,
                                                   CategoryName = g.FirstOrDefault().CategoryName,
                                                   ProductId = g.FirstOrDefault().ProductId,
                                                   Picture = g.FirstOrDefault().Picture,
                                                   Color = g.FirstOrDefault().Color,
                                                   Price = g.FirstOrDefault().Price,
                                                   CategoryDescription = g.FirstOrDefault().CategoryDescription
                                               }).ToList();

            var ReviewList = (from re in db.Reviews
                              group re by re.ProductId into g
                              select new ReviewProduction()
                              {
                                  ProductId = g.Key,
                                  OverallRating = (short)(g.Sum(item => item.OverallRating) / g.Count()),
                                  Count = g.Count()
                              }).ToList();
           /*  var s = searchList.searchProductionList.SelectMany
            (
                p => ReviewList.Where(re => p.ProductId == re.ProductId).DefaultIfEmpty(),
                (p, re) => new
                {
                    p, re
                }
            ).ToList().Select(x => new SearchProduction()
            {
                Name = x.p.Name,
                Description = x.p.Description,
                CategoryName = x.p.Name,
                ProductId = x.p.ProductId,
                Picture = x.p.Picture,
                Color = x.p.Color,
                Price = x.p.Price,
                CategoryDescription = x.p.CategoryDescription,
                OverallRating = x.re.OverallRating,
                Count = x.re.Count
            }).ToList(); ;

            */
            var s = searchList.searchProductionList.Join(ReviewList,
                    p => p.ProductId,
                    re => re.ProductId,

                    (p, re) => new { p, re }).Select(x => new SearchProduction()
                    {
                        Name = x.p.Name,
                        Description = x.p.Description,
                        CategoryName = x.p.Name,
                        ProductId = x.p.ProductId,
                        Picture = x.p.Picture,
                        Color = x.p.Color,
                        Price = x.p.Price,
                        CategoryDescription = x.p.CategoryDescription,
                        OverallRating = x.re.OverallRating,
                        Count = x.re.Count
                    }).ToList();
                    
            //    searchList.searchProductionList = searchList.searchProductionList.GroupBy(c => c.ProductId);

            // var s =  searchList.searchProductionList.Where(t => searchArr.Any(w => t.Description.ToLower().Contains(w)|| t.Name.ToLower().Contains(w) || t.CategoryName.ToLower().Contains(w) || t.CategoryName.ToLower().Contains(w))).ToList() ;
             searchList.searchProductionList = s;
            int pageSize = 16;
            int pageNumber = 1;

            //return PartialView("Index", searchList.searchProductionList.ToPagedList(pageNumber, pageSize));
            return View("Index", searchList.searchProductionList.ToPagedList(pageNumber, pageSize));
        }


    }}

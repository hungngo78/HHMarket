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
    public class ProductionController : Controller
    {
        
        public ActionResult ProductDetail(int id)
        {
            DBModelContainer db = new DBModelContainer();
            ProductionDetail pr = new ProductionDetail();
            pr.listdata = (from p in db.ProductDetails
                                   join production in db.Products on p.ProductId equals production.ProductId
                                   where p.ProductId == id 
                                   select new ProductionDetail(){ Color =p.Color, Description= production.Description,
                                   Price =p.Price, Size = p.Size, Amount = p.Amount, Picture= p.Picture,
                                       ProductDetailsId =p.ProductDetailsId, ProductId = p.ProductId,
                                   ProductionName = production.Name}).ToList();

            // return View(productionDetail.FirstOrDefault());
            pr.reviewListData = (from r in db.Reviews
                                 join p in db.Products on r.ProductId equals p.ProductId
                                 join u in db.Users on r.UserId equals u.UserId
                                 where p.ProductId == id
                                 select new ReviewProduction()
                                 {
                                     Title = r.Title,
                                     Content = r.Content,
                                     OverallRating = r.OverallRating,
                                     ProductId = r.ProductId,
                                     ReviewId = r.ReviewId,
                                     UserId = r.UserId,
                                     UserName = u.UserName,
                                     ReviewDate = r.ReviewDate
                                 }).ToList(); 
                                 
            return View(pr);
        }


    }
}

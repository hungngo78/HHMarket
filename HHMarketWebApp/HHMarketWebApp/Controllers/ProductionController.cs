﻿using System;
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
using PagedList;

namespace HHMarketWebApp.Controllers
{
    public class ProductionController : Controller
    {
        public ActionResult Index(int categoryId, int? page)
        {
            DBModelContainer db = new DBModelContainer();

            List<Production> prs = (from p in db.Products
                                    join pDetail in db.ProductDetails on  p.ProductId equals pDetail.ProductId 
                                    where p.CategoryId == categoryId
                                    select new
                                    {
                                        p,
                                        pDetail
                                    } into t1

                                    group t1 by t1.p.ProductId into g
                                    select new Production()
                                    {
                                        ProductId = g.Key,
                                        ProductionName = g.FirstOrDefault().p.Name,
                                        Description = g.FirstOrDefault().p.Description,
                                        MinPrice = g.Min(m => m.pDetail.Price),
                                        MaxPrice = g.Max(m => m.pDetail.Price),
                                        Picture = g.FirstOrDefault().pDetail.Picture,
                                        Color = g.FirstOrDefault().pDetail.Color
                                    }).ToList(); 

            int pageSize = 16;
            int pageNumber = (page ?? 1);

            return View(prs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ReviewProduction(int id)
        {
            DBModelContainer db = new DBModelContainer();
            ReviewDetails reviewDetails = new ReviewDetails();

            Production production = (from p in db.Products
                                     join pDetail in db.ProductDetails on p.ProductId equals pDetail.ProductId
                                     where p.ProductId == id
                                     select new
                                     {
                                         p.ProductId,
                                         p.Name,
                                         pDetail.Price,
                                         pDetail.Picture,
                                         pDetail.Color
                                     } into t1
                                     group t1 by t1.ProductId into g
                                     select new Production
                                     {
                                         ProductId = g.Key,
                                         ProductionName = g.FirstOrDefault().Name,
                                         MinPrice = g.Min(m => m.Price),
                                         MaxPrice = g.Max(m => m.Price),
                                         Picture = g.FirstOrDefault().Picture,
                                         Color = g.FirstOrDefault().Color
                                     }).FirstOrDefault();
            reviewDetails.product = production;

            List<ReviewProduction> reviewList = (from r in db.Reviews
                                    join u in db.Users on r.UserId equals u.UserId
                                    where r.ProductId == id
                                    select new ReviewProduction
                                    {
                                        ReviewId = r.ReviewId,
                                        Title = r.Title,
                                        Content = r.Content,
                                        OverallRating = r.OverallRating,
                                        UserName = u.UserName,
                                        ReviewDate = r.ReviewDate,
                                    }).ToList();
            reviewDetails.reviewList = reviewList;

            var reviewNumbers = (from r in reviewList
                                 group r by new { r.OverallRating } into g
                                 select new
                                 {
                                     OverrallRating = g.Key.OverallRating,
                                     Count = g.Count()
                                 }).ToList();

            Rating rating = new Rating();

            // calculate number and percent of each rate
            foreach (var reviewNumber in reviewNumbers) { 
                if (reviewNumber.OverrallRating == 1)
                    rating.oneStarReviewNumber = reviewNumber.Count;
                else if (reviewNumber.OverrallRating == 2)
                    rating.twoStarReviewNumber = reviewNumber.Count;
                else if (reviewNumber.OverrallRating == 3)
                    rating.threeStarReviewNumber = reviewNumber.Count;
                else if (reviewNumber.OverrallRating == 4)
                    rating.fourStarReviewNumber = reviewNumber.Count;
                else if (reviewNumber.OverrallRating == 5)
                    rating.fiveStarReviewNumber = reviewNumber.Count;
            }
            rating.oneStarReviewPercent = Convert.ToDecimal(String.Format("{0:0.0000}", rating.oneStarReviewNumber*100/ reviewList.Count()));
            rating.twoStarReviewPercent = Convert.ToDecimal(String.Format("{0:0.0000}", rating.twoStarReviewNumber*100/ reviewList.Count()));
            rating.threeStarReviewPercent = Convert.ToDecimal(String.Format("{0:0.0000}", rating.threeStarReviewNumber*100/ reviewList.Count()));
            rating.fourStarReviewPercent = Convert.ToDecimal(String.Format("{0:0.0000}", rating.fourStarReviewNumber*100/ reviewList.Count()));
            rating.fiveStarReviewPercent = Convert.ToDecimal(String.Format("{0:0.0000}", rating.fiveStarReviewNumber*100/ reviewList.Count()));

            // calculate average overral rating
            decimal overrall = 0;
            foreach (ReviewProduction review in reviewList)
            {
                overrall += review.OverallRating;
            }
            overrall = overrall / reviewList.Count();
            overrall = Convert.ToDecimal(String.Format("{0:0.0}", overrall));
            rating.overrallRating = overrall;

            reviewDetails.rating = rating;

            // render View
            return View(reviewDetails);
        }

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
                                     ProductId = r.ProductId,

                                     Title = r.Title,
                                     Content = r.Content,
                                     OverallRating = r.OverallRating,
                                     
                                     ReviewId = r.ReviewId,
                                     UserId = r.UserId,
                                     UserName = u.UserName,
                                     ReviewDate = r.ReviewDate
                                 }).ToList(); 
                                 
            return View(pr);
        }
    }
}

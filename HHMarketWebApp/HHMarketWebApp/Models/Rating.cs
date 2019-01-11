using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HHMarketWebApp.Models
{
    public class Rating
    {
        public Rating()
        {
            oneStarReviewNumber = 0;
            twoStarReviewNumber = 0;
            threeStarReviewNumber = 0;
            fourStarReviewNumber = 0;
            fiveStarReviewNumber = 0;

            oneStarReviewPercent = 0;
            twoStarReviewPercent = 0;
            threeStarReviewPercent = 0;
            fourStarReviewPercent = 0;
            fiveStarReviewPercent = 0;
        }

        public int oneStarReviewNumber { get; set; }
        public int twoStarReviewNumber { get; set; }
        public int threeStarReviewNumber { get; set; }
        public int fourStarReviewNumber { get; set; }
        public int fiveStarReviewNumber { get; set; }

        public decimal oneStarReviewPercent { get; set; }
        public decimal twoStarReviewPercent { get; set; }
        public decimal threeStarReviewPercent { get; set; }
        public decimal fourStarReviewPercent { get; set; }
        public decimal fiveStarReviewPercent { get; set; }

        public decimal overrallRating { get; set; }
    }
}
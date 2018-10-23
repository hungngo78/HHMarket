namespace HHMarketWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Review")]
    public partial class Review
    {
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }

        public short OverallRating { get; set; }

        public DateTime ReviewDate { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
}

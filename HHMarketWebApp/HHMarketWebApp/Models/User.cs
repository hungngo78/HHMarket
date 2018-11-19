namespace HHMarketWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
            Reviews = new HashSet<Review>();
        }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter an User name")]
        [Display(Name = "User Name:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [Display(Name = "Password:")]
        public string Password { get; set; }

        [EmailAddress(ErrorMessage = "The Email Address is not valid")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [Display(Name = "Email Address:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter the user's last name.")]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter the user's last name.")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter the user's Address.")]
        [Display(Name = "Address:")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter the user's city.")]
        [Display(Name = "City:")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter the user's state.")]
        [Display(Name = "State:")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter the user's zipcode.")]
        [Display(Name = "Zipcode:")]
        public string ZipCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Product> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Review> Reviews { get; set; }
    }
}

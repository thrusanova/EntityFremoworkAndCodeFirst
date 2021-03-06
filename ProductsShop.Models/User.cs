﻿
namespace ProductsShop.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        public User()
        {
            this.ProductsBought = new HashSet<Product>();
            this.ProductsSold = new HashSet<Product>();
            this.Friends = new HashSet<User>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Friends { get; set; }

        [JsonIgnore]
        [InverseProperty("Seller")]
        public virtual ICollection<Product> ProductsSold { get; set; }

        [JsonIgnore]
        [InverseProperty("Buyer")]
        public virtual ICollection<Product> ProductsBought { get; set; }


    }
}

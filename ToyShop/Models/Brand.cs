using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ToyShop.Models
{
    public class Brand
    {
        [Key]
        public long BrandId { get; set; }
        [Required]
        public string BrandName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyShop.Models
{
    public class Card
    {
        [Key]
        public int CartID { get; set; }

        public long ProductID { get; set; }
        public int Quantity { get; set; }
        public virtual Product Product { get; set; }
    }
}
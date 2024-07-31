using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace ToyShop.Models
{
    public class ToyShopDBContext: DbContext
    {
        public ToyShopDBContext() : base("ToyShopConectionString") { }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> User { get; set; }
    }
}
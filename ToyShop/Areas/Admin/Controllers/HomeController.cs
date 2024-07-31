using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyShop.Models;

namespace ToyShop.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index(string search = "", int page = 1)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            //search
            List<Product> pr = db.Products
                .Where(row => row.ProductName.Contains(search) || row.Category.CategoryName.Contains(search))
                .ToList();
            ViewBag.Search = search;

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();
            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            int prperpage = 6;
            int prpages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(pr.Count) / Convert.ToDouble(prperpage)));
            int skippages = (page - 1) * prperpage;
            ViewBag.Page = page;
            ViewBag.Pages = prpages;
            pr = pr.Skip(skippages).Take(prperpage).ToList();
            return View(pr);
        }
        public ActionResult DSSP(long? brandId, long? categoryId, string search = "", string SortColumn = "", int page = 1)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Product> pr = new List<Product>();

            if (brandId != null)
            {
                pr = db.Products.Where(p => p.BrandID == brandId).ToList();
                ViewBag.SelectedBrand = db.Brands.Find(brandId)?.BrandName;
                ViewBag.SelectedBrandId = db.Brands.Find(brandId)?.BrandId;
            }
            else if (categoryId != null)
            {
                pr = db.Products.Where(p => p.CategoryID == categoryId).ToList();
                ViewBag.SelectedCategory = db.Categories.Find(categoryId)?.CategoryName;
                ViewBag.SelectedCategoryId = db.Categories.Find(categoryId)?.CategoryId;
            }
            else if (search != "")
            {
                pr = db.Products
               .Where(row => row.ProductName.Contains(search) || row.Category.CategoryName.Contains(search))
               .ToList();
                ViewBag.Search = search;
            }
            else
            {
                // Lấy tất cả sản phẩm nếu không có thương hiệu hoặc danh mục được chọn
                pr = db.Products.ToList();
            }

            ViewBag.SortColumn = SortColumn;
            if (SortColumn == "PriceUp")
            {
                pr = pr.OrderBy(row => row.ProductPrice).ToList();
            }
            else if (SortColumn == "PriceDown")
            {

                pr = pr.OrderByDescending(row => row.ProductPrice).ToList();
            }
            else if (SortColumn == "A-Z")
            {

                pr = pr.OrderBy(row => row.ProductName).ToList();
            }
            else if (SortColumn == "Z-A")
            {

                pr = pr.OrderByDescending(row => row.ProductName).ToList();
            }

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            int prperpage = 6;
            int prpages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(pr.Count) / Convert.ToDouble(prperpage)));
            int skippages = (page - 1) * prperpage;
            ViewBag.Page = page;
            ViewBag.Pages = prpages;
            pr = pr.Skip(skippages).Take(prperpage).ToList();

            return View(pr);
        }
        public ActionResult Create()
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, List<HttpPostedFileBase> uploadFile)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            db.Products.Add(product);
            db.SaveChanges();
            Product pr = db.Products.ToList().Last();
            Image image = new Image();
            foreach (var item in uploadFile)
            {
                string filePath = Path.Combine(HttpContext.Server.MapPath("~/ImageSP"), Path.GetFileName(item.FileName));
                item.SaveAs(filePath);

                string fileName = item.FileName;
                image.ImageUrl = fileName;
                image.ProductID = pr.ProductID;
                db.Image.Add(image);
                db.SaveChanges();
            }
            db.SaveChanges();
            return RedirectToAction("Create");
        }
        public ActionResult Detail(long id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            Product pr = db.Products.Where(d => d.ProductID == id).FirstOrDefault();
            Brand bra = db.Brands.Where(d => d.BrandId == pr.BrandID).FirstOrDefault();
            Category ctgo = db.Categories.Where(d => d.CategoryId == pr.CategoryID).FirstOrDefault();

            ViewBag.Bra = bra;
            ViewBag.Ctgo = ctgo;

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;
            return View(pr);
        }

        public ActionResult Delete(long id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            Product pr = db.Products.Where(row => row.ProductID == id).FirstOrDefault();

            Brand bra = db.Brands.Where(d => d.BrandId == pr.BrandID).FirstOrDefault();
            Category ctgo = db.Categories.Where(d => d.CategoryId == pr.CategoryID).FirstOrDefault();

            ViewBag.Bra = bra;
            ViewBag.Ctgo = ctgo;

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();
            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View(pr);
        }
        [HttpPost]
        public ActionResult Deleted(long id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            Product pr = db.Products.Where(row => row.ProductID == id).FirstOrDefault();


            List<Image> imagesToDelete = db.Image.Where(i => i.ProductID == id).ToList();


            foreach (var imageToDelete in imagesToDelete)
            {
                // Xóa tệp tin từ hệ thống tệp tin
                string filePath = Path.Combine(Server.MapPath("~/ImageSP"), imageToDelete.ImageUrl);


                System.IO.File.Delete(filePath);

                // Xóa ảnh từ DbSet
                db.Image.Remove(imageToDelete);
            }

            db.Products.Remove(pr);
            db.SaveChanges();

            return RedirectToAction("DSSP");
        }
        public ActionResult Edit(long id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            Product pr = db.Products.Where(d => d.ProductID == id).FirstOrDefault();
            Brand bra = db.Brands.Where(d => d.BrandId == pr.BrandID).FirstOrDefault();
            Category ctgo = db.Categories.Where(d => d.CategoryId == pr.CategoryID).FirstOrDefault();

            ViewBag.Bra = bra;
            ViewBag.Ctgo = ctgo;

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;
            return View(pr);
        }
        [HttpPost]
        public ActionResult Edit(Product e)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            Product pr = db.Products.Where(d => d.ProductID == e.ProductID).FirstOrDefault();

            pr.ProductName = e.ProductName;
            pr.ProductPrice = e.ProductPrice;
            pr.ProductDescription = e.ProductDescription;
            pr.BrandID = e.BrandID;
            pr.CategoryID = e.CategoryID;
            db.SaveChanges();
            return RedirectToAction("DSSP");
        }
    }
}
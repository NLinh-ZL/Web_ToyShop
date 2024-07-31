using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyShop.Models;
using BCrypt;
using System.Web.UI;

namespace ToyShop.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        // GET: Admin/User
        public ActionResult Index(int page = 1)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<User> us = db.User.ToList();

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            int usperpage = 6;
            int uspages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(us.Count) / Convert.ToDouble(usperpage)));
            int skippages = (page - 1) * usperpage;
            ViewBag.Page = page;
            ViewBag.Pages = uspages;
            us = us.Skip(skippages).Take(usperpage).ToList();

            return View(us);
        }
        public ActionResult Add()
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View();
        }
        [HttpPost]
        public ActionResult Add(User user, string retypePassword)
        {

            ToyShopDBContext db = new ToyShopDBContext();
            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (user.Password != retypePassword)
            {
                ModelState.AddModelError("retypePassword", "Mật khẩu không khớp.");
                return View();
            }

            User myUser = db.User.Where(u => u.UserName == user.UserName).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại.");
                return View();
            }

            myUser = db.User.Where(u => u.EmailAddress == user.EmailAddress).FirstOrDefault();
            if (myUser != null)
            {
                ModelState.AddModelError("EmailAddress", "Email đã tồn tại.");
                return View();
            }

            myUser = new User();
            myUser.UserName = user.UserName;
            myUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            myUser.EmailAddress = user.EmailAddress;
            myUser.Role = "user";
            db.User.Add(myUser);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            User us = db.User.Where(row => row.UserId == id).FirstOrDefault();

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();
            ViewBag.Br = br;
            ViewBag.Ctg = ctg;
            return View(us);
        }
        [HttpPost]
        public ActionResult Deleted(int id)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            User us = db.User.Where(row => row.UserId == id).FirstOrDefault();

            db.User.Remove(us);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ToyShop.Models;
using BCrypt;

namespace ToyShop.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                ToyShopDBContext db = new ToyShopDBContext();
                User myUser = db.User.Where(u => u.UserName == user.UserName).FirstOrDefault();
                if (myUser != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(user.Password, myUser.Password))
                    {
                        HttpCookie authCookie = new HttpCookie("auth", myUser.UserName);
                        HttpCookie roleCookie = new HttpCookie("role", myUser.Role);
                        Response.Cookies.Add(authCookie);
                        Response.Cookies.Add(roleCookie);

                        if (myUser.Role == "admin")
                        {
                            // Nếu vai trò là "Admin", chuyển hướng tới action trong khu vực "Admin"
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            // Nếu vai trò không phải là "Admin", chuyển hướng tới action bình thường
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                ModelState.AddModelError("Password", "Invalid Username or Password !!!");
            }
            return View();
        }

        public ActionResult Register()
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View();
        }
        [HttpPost]
        public ActionResult Register(User user, string retypePassword)
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

            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            HttpCookie authcookie = new HttpCookie("auth");
            authcookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(authcookie);

            HttpCookie roleCookie = new HttpCookie("role");
            roleCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(roleCookie);

            return RedirectToAction("Index", "Home");
        }
    }

}
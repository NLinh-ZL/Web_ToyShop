using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToyShop.Models;

namespace ToyShop.Controllers
{
    public class CardController : Controller
    {
        // GET: Card
        public ActionResult Index()
        {
            ToyShopDBContext db = new ToyShopDBContext();
            List<Card> cards = db.Cards.ToList();

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return View(cards);
        }
        public ActionResult Add(long? id)
        {
            if (id.HasValue)
            {
                ToyShopDBContext db = new ToyShopDBContext();
                Card cartitem = db.Cards.Where(row => row.ProductID == id).FirstOrDefault();
                if (cartitem != null)
                {
                    cartitem.Quantity += 1;
                }
                else
                {
                    Card card = new Card();
                    card.ProductID = (int)id;
                    card.Quantity = 1;
                    db.Cards.Add(card);
                }
                db.SaveChanges();

                List<Brand> br = db.Brands.ToList();
                List<Category> ctg = db.Categories.ToList();

                ViewBag.Br = br;
                ViewBag.Ctg = ctg;

            }
            return RedirectToAction("index");
        }
        public ActionResult UpdateQuantity(int quan, int proid)
        {
            ToyShopDBContext db = new ToyShopDBContext();
            if (quan > 0)
            {
                Card cards = db.Cards.Where(row => row.ProductID == proid).FirstOrDefault();
                if (cards != null)
                {
                    cards.Quantity = quan;
                    db.SaveChanges();
                }
            }

            List<Brand> br = db.Brands.ToList();
            List<Category> ctg = db.Categories.ToList();

            ViewBag.Br = br;
            ViewBag.Ctg = ctg;

            return RedirectToAction("index");
        }
        public ActionResult Delete(int id)
        {
            ToyShopDBContext db = new ToyShopDBContext();

            // Tìm mục trong giỏ hàng dựa trên ID
            Card cardToDelete = db.Cards.FirstOrDefault(c => c.ProductID == id);

            if (cardToDelete != null)
            {
                // Xóa mục khỏi DbSet và lưu thay đổi vào cơ sở dữ liệu
                db.Cards.Remove(cardToDelete);
                db.SaveChanges();
            }

            // Redirect về trang giỏ hàng sau khi xóa
            return RedirectToAction("Index");
        }

    }
}
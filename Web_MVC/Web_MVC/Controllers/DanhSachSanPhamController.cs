using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class DanhSachSanPhamController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: DanhSachSanPham
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                List<Product> products = db.Products.ToList();
                return View(products);
            }else
            {
                List<Product> products = db.Products.Where(m => m.ProductsCatalogID == id).ToList();
                return View(products);
            }
            
        }

        public ActionResult ChiTiet(int id)
        {
            Product product = db.Products.Find(id);
            return View(product);
        }
    }
}
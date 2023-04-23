using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web_MVC.Models;
namespace Web_MVC.Areas.Admin.Controllers
{
    public class DanhMucSanPhamController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/DanhMucSanPham
        public ActionResult Index(int ? page)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            // Lấy danh sách sản phẩm từ CSDL
            List<ProductsCatalog> productsCatalog = db.ProductsCatalogs.OrderBy(p => p.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Truyền số trang hiện tại đến view để hiển thị phân trang
            ViewBag.CurrentPage = pageNumber;

            // Truyền tổng số trang đến view để hiển thị phân trang
            ViewBag.TotalPages = Math.Ceiling((double)db.ProductsCatalogs.Count() / pageSize);

            return View(productsCatalog);
        }

        public ActionResult ThemMoi()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ThemMoi(ProductsCatalog model)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var productCatalog = db.ProductsCatalogs.Any(m => m.Name == model.Name);
                if (productCatalog)
                {
                    ModelState.AddModelError("", "Tên danh mục đã tồn tại");
                    return View(model);
                }
                else
                {
                    db.ProductsCatalogs.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("", "Thông tin không được để trống!");
            return View(model);
        }

        public ActionResult Sua(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            ProductsCatalog productsCatalog = db.ProductsCatalogs.Find(id);
            return View(productsCatalog);
        }

        [HttpPost]
        public ActionResult Sua(ProductsCatalog model, int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checkProductsCatalog = db.ProductsCatalogs.Any(m => m.Name == model.Name && m.ID != model.ID);
                if (checkProductsCatalog)
                {
                    ModelState.AddModelError("", "Tên danh mục đã tồn tại");
                    return View(model);
                }
                ProductsCatalog productsCatalog = db.ProductsCatalogs.Find(id);
                if (productsCatalog != null)
                {
                    productsCatalog.Name = model.Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    ModelState.AddModelError("", "Sửa danh mục thất bại");
                    return View(model);
                }
            } else
            {
                ModelState.AddModelError("", "Thông tin không được để trống");
                return View(model);
            }
        }

        public ActionResult Xoa(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            ProductsCatalog productsCatalog = db.ProductsCatalogs.Find(id);
            if (productsCatalog != null)
            {
                db.ProductsCatalogs.Remove(productsCatalog);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                ViewBag.error = "Xoá người danh mục thất bại";
                return View();
            }
        }
    }
}
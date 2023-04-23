using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class DanhMucTinTucController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/DanhMucTinTuc
        public ActionResult Index(int ? page)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            List<NewsCatalog> newsCatalogs = db.NewsCatalogs.OrderBy(p => p.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Truyền số trang hiện tại đến view để hiển thị phân trang
            ViewBag.CurrentPage = pageNumber;

            // Truyền tổng số trang đến view để hiển thị phân trang
            ViewBag.TotalPages = Math.Ceiling((double)db.NewsCatalogs.Count() / pageSize);
            return View(newsCatalogs);
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
        public ActionResult ThemMoi(NewsCatalog model)
        {
            if (ModelState.IsValid == true)
            {
                if (Session["UserName"] == null)
                {
                    return RedirectToAction("index", "Login");
                }
                var newCatalog = db.NewsCatalogs.Any(m => m.Name.ToLower() == model.Name.ToLower());
                if (newCatalog)
                {
                    ModelState.AddModelError("", "Danh mục tin tức đã tồn tại");
                    return View(model);
                }
                db.NewsCatalogs.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            NewsCatalog newsCatalog = db.NewsCatalogs.Find(id);
            if (newsCatalog != null)
            {
                db.NewsCatalogs.Remove(newsCatalog);
                db.SaveChanges();
                return RedirectToAction("index");
            } else
            {
                ViewBag.error = "Xoá thất bại";
                return RedirectToAction("index");
            }
        }

        public ActionResult Sua(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            NewsCatalog newsCatalog = db.NewsCatalogs.Find(id);
            return View(newsCatalog);
        }

        [HttpPost]
        public ActionResult Sua(NewsCatalog model, int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var newCatalog = db.NewsCatalogs.Any(m => m.Name.ToLower() == model.Name.ToLower() && m.ID != model.ID);
                if (newCatalog)
                {
                    ModelState.AddModelError("", "Danh mục tin tức đã tồn tại");
                    return View(model);
                }
                NewsCatalog newsCatalog = db.NewsCatalogs.Find(id);
                if (newsCatalog != null)
                {
                    newsCatalog.Name = model.Name;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    ModelState.AddModelError("", "Thêm danh mục tin tức thất bại");
                    return View(model);
                }
            } else
            {
                ModelState.AddModelError("", "Thông tin không được để trống");
                return View(model);
            }
        }
    }
}
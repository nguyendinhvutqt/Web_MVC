using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class TinTucController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/TinTuc
        public ActionResult Index(int? page)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            // Thiết lập kích thước trang và số trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            // Lấy danh sách sản phẩm từ CSDL
            List<News> news = db.News.OrderBy(p => p.Title).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Truyền số trang hiện tại đến view để hiển thị phân trang
            ViewBag.CurrentPage = pageNumber;

            // Truyền tổng số trang đến view để hiển thị phân trang
            ViewBag.TotalPages = (int)Math.Ceiling((double)db.News.Count() / pageSize);
            return View(news);
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
        [ValidateInput(false)]
        public ActionResult ThemMoi(News model, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var news = db.News.Any(m => m.Title == model.Title);
                if (news)
                {
                    ModelState.AddModelError("", "Tiêu đề bài viết đã tồn tại");
                    return View(model);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/tintuc"), fileName);
                        file.SaveAs(path);
                        var newNews = new News
                        {
                            Title = model.Title,
                            Description = model.Description,
                            Contents = form["Contents"],
                            Image = "~/Areas/Admin/assets/img/tintuc/" + file.FileName,
                            NewsCatalogID = model.NewsCatalogID
                        };
                        db.News.Add(newNews);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    } else
                    {
                        ModelState.AddModelError("", "File hình ảnh không hợp lệ");
                        return View(model);
                    }
                } else
                {
                    ModelState.AddModelError("", "File hình ảnh là bắt buộc");
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
            News news = db.News.Find(id);
            if (news != null)
            {
                db.News.Remove(news);
                db.SaveChanges();
                return RedirectToAction("index");
            } else
            {
                ViewBag.error = "Xoá thất bại";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Sua(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            News news = db.News.Find(id);
            return View(news);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(News model, int id, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checkNews = db.News.Any(m => m.Title == model.Title && m.ID != model.ID);
                if (checkNews)
                {
                    ModelState.AddModelError("", "Tiêu đề đã tồn tại");
                    return View(model);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/tintuc"), fileName);
                        file.SaveAs(path);
                        News news = db.News.Find(id);
                        if (news != null)
                        {
                            news.Title = model.Title;
                            news.Description = model.Description;
                            news.Contents = form["Contents"];
                            news.Image = "~/Areas/Admin/assets/img/tintuc/" + fileName;
                            news.NewsCatalogID = model.NewsCatalogID;
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "File hình ảnh không hợp lệ");
                        return View(model);
                    }

                }
                else
                {
                    News news = db.News.Find(id);
                    if (news != null)
                    {
                        news.Title = model.Title;
                        news.Description = model.Description;
                        news.Contents = model.Contents;
                        news.NewsCatalogID = model.NewsCatalogID;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                        return View(model);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Thông tin không được để trống!");
                return View(model);
            }
        }
    }
}
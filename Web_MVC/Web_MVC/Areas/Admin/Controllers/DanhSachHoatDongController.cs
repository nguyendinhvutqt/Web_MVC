using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class DanhSachHoatDongController : Controller
    {
        private Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/DanhSachHoatDong
        public ActionResult Index(int ? page)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            // Thiết lập kích thước trang và số trang
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            // Lấy danh sách sản phẩm từ CSDL
            List<Activity> activities = db.Activities.OrderBy(p => p.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Truyền số trang hiện tại đến view để hiển thị phân trang
            ViewBag.CurrentPage = pageNumber;

            // Truyền tổng số trang đến view để hiển thị phân trang
            ViewBag.TotalPages = Math.Ceiling((double)db.Activities.Count() / pageSize);
            return View(activities);
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
        public ActionResult ThemMoi(Activity model, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checkActivity = db.Activities.Any(m => m.Title == model.Title);
                if (checkActivity)
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
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/hoatdong"), fileName);
                        file.SaveAs(path);
                        var newActivity = new Activity
                        {
                            Title = model.Title,
                            Description = form["Description"],
                            Image = "~/Areas/Admin/assets/img/hoatdong/" + file.FileName,
                        };
                        db.Activities.Add(newActivity);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "File hình ảnh không hợp lệ");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "File hình ảnh là bắt buộc");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng điển đầy đủ thông tin!");
                return View(model);
            }
        }

        public ActionResult ChiTiet(int id)
        {
            Activity activity = db.Activities.Find(id);
            return View(activity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChiTiet(Activity model, int id, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checActivity = db.Activities.Any(m => m.Title == model.Title && m.ID != model.ID);
                if (checActivity)
                {
                    ModelState.AddModelError("", "Tên hoạt động đã tồn tại");
                    return View(model);
                }
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/hoatdong"), fileName);
                        file.SaveAs(path);
                        Activity activity = db.Activities.Find(id);
                        if (activity != null)
                        {
                            activity.Title = model.Title;
                            activity.Description = form["Description"];
                            activity.Image = "~/Areas/Admin/assets/img/hoatdong/" + fileName;
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
                    Activity activity = db.Activities.Find(id);
                    if (activity != null)
                    {
                        activity.Title = model.Title;
                        activity.Description = model.Description;
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

        public ActionResult Xoa(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            var activity = db.Activities.Find(id);
            if (activity != null)
            {
                db.Activities.Remove(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Xoá sản phẩm thất bại";
                return View();
            }
        }
    }
}
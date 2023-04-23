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
    public class SanPhamController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/SanPham
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
            List<Product> products = db.Products.OrderBy(p => p.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Truyền số trang hiện tại đến view để hiển thị phân trang
            ViewBag.CurrentPage = pageNumber;

            // Truyền tổng số trang đến view để hiển thị phân trang
            ViewBag.TotalPages = Math.Ceiling((double)db.Products.Count() / pageSize);

            return View(products);
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
        public ActionResult ThemMoi(Product model, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checkProduct1 = db.Products.Any(m => m.Code == model.Code);
                if (checkProduct1)
                {
                    ModelState.AddModelError("", "Mã Code đã tồn tại");
                    return View(model);
                }
                var checkProduct = db.Products.Any(m=>m.Name == model.Name);
                if (checkProduct)
                {
                    ModelState.AddModelError("", "Tên sản phẩm đã tồn tại!");
                    return View(model);
                }

                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/products"), fileName);
                        file.SaveAs(path);
                        var newProduct = new Product
                        {
                            Code = model.Code,
                            Name = model.Name,
                            Description = form["Description"],
                            Image = "~/Areas/Admin/assets/img/products/" + file.FileName,
                            ProductsCatalogID = model.ProductsCatalogID
                        };
                        db.Products.Add(newProduct);
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
            } else
            {
                ModelState.AddModelError("", "Vui lòng điển đầy đủ thông tin!");
                return View(model);
            }
        }

        public ActionResult Xoa(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            var product = db.Products.Find(id);
            if (product != null)
            {
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            } else
            {
                ViewBag.error = "Xoá sản phẩm thất bại";
                return View();
            }
        }

        public ActionResult Sua(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            Product product = db.Products.Find(id);
            return View(product);
        }

        

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sua(Product model, int id, HttpPostedFileBase file, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                var checkProduct = db.Products.Any(m => m.Name == model.Name && m.ID != model.ID);
                if (checkProduct)
                {
                    ModelState.AddModelError("", "Tên sản phẩm đã tồn tại");
                    return View(model);
                }
                var checkProduct1 = db.Products.Any(m => m.Code == model.Code && m.ID != model.ID);
                if (checkProduct1)
                {
                    ModelState.AddModelError("", "Mã Code đã tồn tại");
                    return View(model);
                }
                
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(file.FileName).ToLower();
                    if (allowedExtensions.Contains(fileExtension))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/assets/img/products"), fileName);
                        file.SaveAs(path);
                        Product product = db.Products.Find(id);
                        if (product != null)
                        {
                            product.Code = model.Code;
                            product.Name = model.Name;
                            product.Description = form["Description"];
                            product.Image = "~/Areas/Admin/assets/img/products/" + fileName;
                            product.ProductsCatalogID = model.ProductsCatalogID;
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
                    
                } else
                {
                    Product product = db.Products.Find(id);
                    if (product != null)
                    {
                        product.Code = model.Code;
                        product.Name = model.Name;
                        product.Description = model.Description;
                        product.ProductsCatalogID = model.ProductsCatalogID;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật thông tin thất bại");
                        return View(model);
                    }
                }
            } else
            {
                ModelState.AddModelError("", "Thông tin không được để trống!");
                return View(model);
            }
        }
    }
}
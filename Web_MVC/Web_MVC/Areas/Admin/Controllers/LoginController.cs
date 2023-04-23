using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Account model)
        {
            if (ModelState.IsValid == true)
            {
                Account account = db.Accounts.FirstOrDefault(m => m.UserName == model.UserName.ToString() && m.Password == model.Password.ToString());
                if (account == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    return View(model);
                } else
                {
                    Session["UserName"] = model.UserName;
                    return RedirectToAction("Index", "HomeAdmin");
                }
            }
            ModelState.AddModelError("", "Thông tin không được để trống");
            return View();
        }
    }
}
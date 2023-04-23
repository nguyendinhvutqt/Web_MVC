using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            return RedirectToAction("index", "Login");
        }
    }
}
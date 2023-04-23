using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class InfomationController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/Infomation
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            Infomation infomation = db.Infomations.FirstOrDefault();
            return View(infomation);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(Infomation model, FormCollection form)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            if (ModelState.IsValid == true)
            {
                Infomation infomation = db.Infomations.Find(model.ID);
                infomation.Info = form["Info"];
                infomation.MainInfo = form["MainInfo"];
                infomation.SubInfo = form["SubInfo"];
                infomation.Email = model.Email;
                infomation.Phone = model.Phone;
                infomation.Address = model.Address;
                db.SaveChanges();
                return View(infomation);
            } else {
                ModelState.AddModelError("", "Thông tin không được để trống");
                return View(model);
            }
        }
    }
}
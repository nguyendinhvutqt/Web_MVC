using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class HoatDongController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: HoatDong
        public ActionResult Index(int? id)
        {
            Activity activity = db.Activities.Find(id);
            return View(activity);
        }

    }
}
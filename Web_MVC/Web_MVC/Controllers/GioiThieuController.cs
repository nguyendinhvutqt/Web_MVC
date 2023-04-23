using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class GioiThieuController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: GioiThieu
        public ActionResult Index()
        {
            Infomation infomation = db.Infomations.FirstOrDefault();
            return View(infomation);
        }
    }
}
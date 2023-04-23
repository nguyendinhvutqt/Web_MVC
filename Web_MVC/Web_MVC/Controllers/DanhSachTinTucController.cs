using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class DanhSachTinTucController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: TinTuc
        public ActionResult Index()
        {
            List<News> news = db.News.ToList();
            return View(news);
        }

        public ActionResult ChiTiet(int id)
        {
            News news = db.News.Find(id);
            return View(news);
        }
    }
}
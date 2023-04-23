using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Home
        public ActionResult Index()
        {
            Infomation infomation = db.Infomations.FirstOrDefault();
            var products = db.Products.OrderByDescending(p => p.Code).Take(4);
            var news = db.News.OrderByDescending(p => p.ID).Take(4);
            var activities = db.Activities.OrderByDescending(p => p.ID).Take(4);

            ViewBag.Infomation = infomation;
            ViewBag.Products = products;
            ViewBag.News = news;
            ViewBag.Activities = activities;
            return View();
        }

    }
}
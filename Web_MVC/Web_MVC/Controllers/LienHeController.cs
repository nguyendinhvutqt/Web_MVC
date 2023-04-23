using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Controllers
{
    public class LienHeController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: LienHe
        public ActionResult Index()
        {
            Infomation infomation = db.Infomations.FirstOrDefault();
            ViewBag.infomation = infomation;
            return View();
        }

        [HttpPost]
        public ActionResult Index(Contact model)
        {
            Infomation infomation = db.Infomations.FirstOrDefault();
            ViewBag.infomation = infomation;
            if (ModelState.IsValid == true)
            {
                var newContact = new Contact{
                    Name = model.Name,
                    Email = model.Email,
                    Contents = model.Contents,
                    Date = DateTime.Now,
                    Seen = false
                };
                db.Contacts.Add(newContact);
                db.SaveChanges();
                ViewBag.success = "Gửi thông tin thành công";
                return View();
            }
            else
            {
                ModelState.AddModelError("", "Thông tin không được để trống");
                return View(model);
            }
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_MVC.Models;

namespace Web_MVC.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly Project_WebEntities db = new Project_WebEntities();
        // GET: Admin/Contact
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            List<Contact> contacts = db.Contacts.ToList();
            return View(contacts);
        }

        public ActionResult Xem(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            Contact contact = db.Contacts.Find(id);
            contact.Seen = true;
            db.SaveChanges();
            return View(contact);
        }

        public ActionResult Xoa(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("index", "Login");
            }
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
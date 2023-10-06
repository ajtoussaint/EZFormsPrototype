using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.DAL;
using EZFormsPrototype.Models;

namespace EZFormsPrototype.Controllers
{
    public class FieldController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();

        // GET: Field
        public ActionResult Create(int id)
        {
            Form parentForm = db.Forms.Find(id);
            if (parentForm == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentID = parentForm.ID;
            ViewBag.ParentTitle = parentForm.Title;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,FormID,Name,Type")] Field field)
        {
            if(ModelState.IsValid)
            {
                db.Fields.Add(field);
                db.SaveChanges();
                return RedirectToAction("Edit", "Form", new { id = field.FormID });
            }
            return View();
        }
    }
}
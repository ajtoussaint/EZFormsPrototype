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
using EZFormsPrototype.Utility;

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

            //Create a list of possible field types to pass to the type input dropdown
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown("number");

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

        public ActionResult Edit(int? id) 
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if(field == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown("number");
            //TODO: Explain why field is not passed as a variable to view?
            return View(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FormID,Name,Type")] Field field)
        {
            if(ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                //send to the parent form edit page
                return RedirectToAction("Edit", "Form", new { id = field.FormID });
            }
            //go back to edit when invalid model is posted
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown("number");
            return View(field);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Field field = db.Fields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            return View(field);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Field field = db.Fields.Find(id);
            db.Fields.Remove(field);
            db.SaveChanges();
            return RedirectToAction("Edit", "Form", new { id = field.FormID });
        }

        public ActionResult ParentForm(int id)
        {
            return RedirectToAction("Edit", "Form", new { id = id });
        }
    }
}
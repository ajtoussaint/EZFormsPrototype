using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.Utility;
using System.Data.Entity;

namespace EZFormsPrototype.Controllers
{
    public class FlagController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();
        // GET: Flag
        public ActionResult Index()
        {
            return View(db.Flags.ToList());
        }

        public ActionResult Create(int id)
        {
            Field parentField = db.Fields.Find(id);
            if (parentField == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentFieldID = parentField.ID;
            ViewBag.ParentName = parentField.Name;
            ViewBag.ParentFormID = parentField.FormID;
            ViewBag.Level = DropDownListUtility.GetFlagLevelDropdown("warning");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,Name,Message,TriggerExpression,Level,FieldID,FormID")] Flag flag)
        {
            if(ModelState.IsValid)
            {
                db.Flags.Add(flag);
                db.SaveChanges();
                return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
            }
            return RedirectToAction("Create", new { id = flag.FieldID });
        }

        public ActionResult Edit(int id)
        {
            Flag flag = db.Flags.Find(id);
            if(flag == null)
            {
                return HttpNotFound();
            }
            ViewBag.Level = DropDownListUtility.GetFlagLevelDropdown(flag.Level);
            ViewBag.Fields = db.Fields.Where(f => f.FormID == flag.FormID).ToList();
            return View(flag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID")] Flag flag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(flag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
            }
            ViewBag.Level = DropDownListUtility.GetFlagLevelDropdown(flag.Level);
            return View(flag);
        }

        public ActionResult Delete(int id)
        {
            Flag flag = db.Flags.Find(id);
            if(flag == null)
            {
                return HttpNotFound();
            }
            return View(flag);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Flag flag = db.Flags.Find(id);
            db.Flags.Remove(flag);
            db.SaveChanges();
            return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
        }

        public ActionResult ParentField(int id)
        {
            return RedirectToAction("Edit", "Field", new {id = id});
        }
    }
}
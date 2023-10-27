using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.Utility;

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
            /*Field parentField = db.Fields.Find(flag.FieldID);
            if (parentField == null)
            {
                return HttpNotFound();
            }

            ViewBag.ParentFieldID = parentField.ID;
            ViewBag.ParentName = parentField.Name;
            return View();*/
            return RedirectToAction("Create", new { id = flag.FieldID });
        }

        public ActionResult ParentField(int id)
        {
            return RedirectToAction("Edit", "Field", new {id = id});
        }
    }
}
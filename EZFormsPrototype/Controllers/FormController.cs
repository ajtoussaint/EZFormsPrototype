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
using EZFormsPrototype.ViewModels;

namespace EZFormsPrototype.Controllers
{
    public class FormController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();

        // GET: Form
        public ActionResult Index()
        {
            return View(db.Forms.ToList());
        }

        // GET: Form/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // GET: Form/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Form/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(form);
        }

        // GET: Form/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            form.Fields = db.FormFields.Where(f => f.FormID == id).ToList();
            return View(form);
        }

        // POST: Form/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(form);
        }

        // GET: Form/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Form form = db.Forms.Find(id);
            db.Forms.Remove(form);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Fillable(int id)
        {
            FillableForm ViewModel = new FillableForm();
            ViewModel.Form = db.Forms.Find(id);
            ViewModel.Fields = db.FormFields.Where(f => f.FormID == ViewModel.Form.ID).ToList();
            //ViewModel.TableFields = db.TableFields.Where(f => f.FormID == ViewModel.Form.ID).ToList();
            List<Flag> flags = db.Flags.Where(f => f.FormID == ViewModel.Form.ID).ToList();
            foreach(Flag flag in flags)
            {
                flag.CodeExpressions = db.ExpressionBlocks.Where(e => e.FlagID == flag.ID).OrderBy(e => e.Order).Select(e => e.CodeExpression).ToList();
                flag.DependantFieldIDs = db.ExpressionBlocks.Where(e => e.FlagID == flag.ID && e.DependantFieldID1 != 0).Select(e => e.DependantFieldID1)
                    .Union(db.ExpressionBlocks.Where(e => e.FlagID == flag.ID && e.DependantFieldID2 != 0).Select(e => e.DependantFieldID2)).Distinct().ToList();
            }
            ViewModel.Flags = flags;
            foreach(FormField field in ViewModel.Fields)
            {
                //get all flag IDs that need to be updated when the field changes
                field.DependentFlagIDs = db.Flags.Where(f => !f.appearsOnSubmit).Join(db.ExpressionBlocks.Where(e => e.DependantFieldID1 == field.ID || e.DependantFieldID2 == field.ID), flag => flag.ID, eb => eb.FlagID, (flag, eb) => flag.ID).Distinct().ToList();
                //get the table fields for any field that is a table
                if(field.Type == "table")
                {
                    field.TableFields = db.TableFields.Where(f => f.TableID == field.ID).ToList();
                }
            }
            ViewModel.TableFields = db.TableFields.Where(f => f.FormID == ViewModel.Form.ID).ToList();
            foreach(TableField tf in ViewModel.TableFields)
            {
                tf.DependentFlagIDs = db.Flags.Join(db.ExpressionBlocks.Where(e => e.DependantFieldID1 == tf.ID || e.DependantFieldID2 == tf.ID), flag => flag.ID, eb => eb.FlagID, (flag, eb) => flag.ID).Distinct().ToList();
            }

            return View(ViewModel);
        }

        public ActionResult FinalResult(FormCollection form)
        {
            FinalResult res = new FinalResult();
            foreach(string key in form.AllKeys)
            {
                if (form[key] != "")
                    res.addResult(key, form[key]);
            }
            res.FormTitle = form["Form.Title"];
            res.FormDescription = form["Form.Description"];
            return View(res);
        }

        public ActionResult CreateField(int id)
        {
            return RedirectToAction("Create", "Field", new { id = id });
        }

        public ActionResult EditField(int id)
        {
            return RedirectToAction("Edit", "Field", new { id = id });
        }

        public ActionResult DeleteField(int id)
        {
            return RedirectToAction("Delete", "Field", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

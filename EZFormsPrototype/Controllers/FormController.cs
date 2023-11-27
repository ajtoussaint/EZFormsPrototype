using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.ViewModels;
using Microsoft.AspNet.Identity;

namespace EZFormsPrototype.Controllers
{
    public class FormController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();

        // GET: Form
        [Authorize]
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            return View(db.Forms.Where(f => f.userID == userID).ToList());
        }

        // GET: Form/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            var userID = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null || form.userID != userID)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // GET: Form/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Form/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Description")] Form form)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                form.userID = userId;
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(form);
        }

        // GET: Form/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            var userID = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null || form.userID != userID)
            {
                return HttpNotFound();
            }
            form.Fields = db.FormFields.Where(f => f.FormID == id).OrderBy(f => f.FormOrder).ToList();
            return View(form);
        }

        // POST: Form/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Description,Redirect")] FormEditViewModel data)
        {
            var userID = User.Identity.GetUserId();

            Form form = new Form();
            form.ID = data.ID;
            form.Title = data.Title;
            form.Description = data.Description;

            //as no tracking allows the modified state to work
            string id =  db.Forms.AsNoTracking().Where(f => f.ID == form.ID).FirstOrDefault().userID;

            if (ModelState.IsValid && userID == id)
            {
                form.userID = userID;
                db.Entry(form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction(data.Redirect);
            }
            return View(form);
        }

        // GET: Form/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            var userID = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null || form.userID != userID)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();

            Form form = db.Forms.Find(id);
            if(form.userID == userID)
            {
                db.Forms.Remove(form);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [Authorize]
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

        [Authorize]
        public ActionResult FinalResult(FormCollection form, int id)
        {
            Form originalForm = db.Forms.Find(id);

            FinalResult fres = new FinalResult();

            fres.FormTitle = originalForm.Title;
            fres.FormDescription = originalForm.Description;

            //get all the fields and flags associated with this form ID
            List<FormField> fields = db.FormFields.Where(f => f.FormID == id).ToList();

            List<Flag> flags = db.Flags.Where(f => f.FormID == id).ToList();

            foreach(FormField field in fields)
            {
                if(field.Type == "table")
                {
                    TableResult res = new TableResult();
                    res.FlagResults = new List<FlagResult> { };
                    res.Title = field.Name;
                    //get list of table fields
                    List<TableField> tableFields = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
                    res.TableFields = tableFields.Select(tf => tf.Name).ToList();
                    res.FieldValues = new List<List<string>>();
                    //for each table field id
                    foreach (TableField tf in tableFields)
                    {
                        var x = form[tf.ID.ToString()];
                        var y = form[tf.ID.ToString()].Split(',');
                        var z = form[tf.ID.ToString()].Split(',').ToList();
                        List<string> tfres = form[tf.ID.ToString()].Split(',').ToList();
                        res.FieldValues.Add(tfres);
                    }
                    foreach (Flag flag in flags.Where(f => f.FieldID == field.ID))
                    {
                        FlagResult flagres = new FlagResult();
                        flagres.FlagTitle = flag.Name;
                        flagres.FlagResponse = form[flag.ID.ToString()];
                        res.FlagResults.Add(flagres);
                    }
                    fres.addResult(res);
                }
                else
                {
                    Result res = new Result();
                    res.FlagResults = new List<FlagResult> { };
                    res.Title = field.Name;
                    res.Value = form[field.ID.ToString()];
                    foreach (Flag flag in flags.Where(f => f.FieldID == field.ID))
                    {
                        FlagResult flagres = new FlagResult();
                        flagres.FlagTitle = flag.Name;
                        flagres.FlagResponse = form[flag.ID.ToString()];
                        res.FlagResults.Add(flagres);
                    }
                    fres.addResult(res);
                }
            }
            return View(fres);
        }

        [Authorize]
        public ActionResult CreateField(int id)
        {
            return RedirectToAction("Create", "Field", new { id = id });
        }

        [Authorize]
        public ActionResult EditField(int id)
        {
            return RedirectToAction("Edit", "Field", new { id = id });
        }

        [Authorize]
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

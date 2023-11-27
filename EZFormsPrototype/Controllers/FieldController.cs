using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.ViewModels;
using EZFormsPrototype.Utility;
using Microsoft.AspNet.Identity;

namespace EZFormsPrototype.Controllers
{
    public class FieldController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();

        // GET: Field
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,FormID,Name,Type,FormOrder,TableFieldNames,TableFieldTypes")] FormField field)
        {
            var userId = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                field.userID = userId;
                db.FormFields.Add(field);
                db.SaveChanges();
                //Check if the user is creating a table and make subfields if they are
                if (field.Type == "table")
                {
                    for(int i = 0; i<field.TableFieldNames.Count; i++)
                    {
                        //TODO: subfields get messed up by validation
                        TableField tf = new TableField();
                        tf.TableID = field.ID;
                        tf.Name = field.TableFieldNames[i];
                        tf.Type = field.TableFieldTypes[i];
                        tf.FormID = field.FormID;
                        tf.FormOrder = i;
                        db.TableFields.Add(tf);
                        db.SaveChanges();
                    }
                }
                
                return RedirectToAction("Edit", "Form", new { id = field.FormID });
            }
            ViewBag.parentID = field.FormID;
            Form parentForm = db.Forms.Find(field.FormID);
            if (parentForm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentTitle = parentForm.Title;
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown("number");
            return View();
        }

        [Authorize]
        public ActionResult Edit(int? id) 
        {
            var userID = User.Identity.GetUserId();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormField field = db.FormFields.Find(id);
            if(field == null || field.userID != userID)
            {
                return HttpNotFound();
            }
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown(field.Type);
            if(field.Type == "table")
            {
                ViewBag.TableFields = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
            }
            ViewBag.Flags = db.Flags.Where(f => f.FieldID == field.ID).ToList();
            return View(field);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FormID,Name,Type,FormOrder,TableFieldNames,TableFieldTypes,Redirect")] FieldEditViewModel data)
        {
            Field field = new Field();
            field.ID = data.ID;
            field.FormID = data.FormID;
            field.Name = data.Name;
            field.Type = data.Type;
            field.FormOrder = data.FormOrder;

            var userID = User.Identity.GetUserId();
            string id = db.Fields.AsNoTracking().Where(f => f.ID == field.ID).FirstOrDefault().userID;
            
            if (ModelState.IsValid && userID == id)
            {
                field.userID = userID;
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                //TODO: This is terrible
                //Drop any TableFields associated with this FormField
                List<TableField> tfList = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
                db.TableFields.RemoveRange(tfList);
                //Add new table fields as needed
                if (field.Type == "table")
                {
                    //TODO: Prevent this from breaking when a flag is made for the table
                    for (int i = 0; i < data.TableFieldNames.Count; i++)
                    {
                        TableField tf = new TableField();
                        tf.TableID = field.ID;
                        tf.Name = data.TableFieldNames[i];
                        tf.Type = data.TableFieldTypes[i];
                        tf.FormID = field.FormID;
                        tf.FormOrder = i;
                        db.TableFields.Add(tf);
                        db.SaveChanges();
                    }
                }
                //send to the parent form edit page
                return RedirectToAction(data.Redirect);
                //return RedirectToAction("Edit", "Form", new { id = field.FormID });
            }
            //go back to edit when invalid model is posted
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown(field.Type);
            return View(field);
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            var userID = User.Identity.GetUserId(); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormField field = db.FormFields.Find(id);
            if (field == null || field.userID != userID)
            {
                return HttpNotFound();
            }
            Form parentForm = db.Forms.Find(field.FormID);
            ViewBag.ParentForm = parentForm.Title;
            return View(field);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();
            FormField field = db.FormFields.Find(id);
            if(field.userID == userID)
            {
                if (field.Type == "table")
                {
                    //Drop any TableFields and Flags associated with this FormField
                    List<TableField> tfList = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
                    List<Flag> fList = db.Flags.Where(f => f.FieldID == field.ID).ToList();
                    db.TableFields.RemoveRange(tfList);
                    db.Flags.RemoveRange(fList);
                }
                db.Fields.Remove(field);
                db.SaveChanges();
            }
            return RedirectToAction("Edit", "Form", new { id = field.FormID });
        }

        [Authorize]
        public ActionResult CreateFlag(int id)
        {
            return RedirectToAction("Create", "Flag", new { id = id });
        }

        [Authorize]
        public ActionResult EditFlag(int id)
        {
            return RedirectToAction("Edit", "Flag", new { id = id });
        }

        [Authorize]
        public ActionResult DeleteFlag(int id)
        {
            return RedirectToAction("Delete", "Flag", new { id = id });
        }

        [Authorize]
        public ActionResult ParentForm(int id)
        {
            return RedirectToAction("Edit", "Form", new { id = id });
        }
    }
}
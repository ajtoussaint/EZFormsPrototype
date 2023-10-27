﻿using System;
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
            //TODO: move this to the view so I don't have to repeat in post action OR just redirect a bad post to GET
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown("number");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,FormID,Name,Type,FormOrder,TableFieldNames,TableFieldTypes")] FormField field)
        {
            
            if(ModelState.IsValid)
            {
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

        public ActionResult Edit(int? id) 
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormField field = db.FormFields.Find(id);
            if(field == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown(field.Type);
            if(field.Type == "table")
            {
                ViewBag.TableFields = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
            }
            return View(field);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FormID,Name,Type,FormOrder,TableFieldNames,TableFieldTypes")] FormField field)
        {
            if(ModelState.IsValid)
            {
                db.Entry(field).State = EntityState.Modified;
                db.SaveChanges();
                //Drop any TableFields associated with this FormField
                List<TableField> tfList = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
                db.TableFields.RemoveRange(tfList);
                //Add new table fields as needed
                if (field.Type == "table")
                {
                    for (int i = 0; i < field.TableFieldNames.Count; i++)
                    {
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
                //send to the parent form edit page
                return RedirectToAction("Edit", "Form", new { id = field.FormID });
            }
            //go back to edit when invalid model is posted
            ViewBag.Type = DropDownListUtility.GetFieldTypeDropdown(field.Type);
            return View(field);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FormField field = db.FormFields.Find(id);
            if (field == null)
            {
                return HttpNotFound();
            }
            Form parentForm = db.Forms.Find(field.FormID);
            ViewBag.ParentForm = parentForm.Title;
            return View(field);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FormField field = db.FormFields.Find(id);
            if (field.Type == "table")
            {
                //Drop any TableFields associated with this FormField
                List<TableField> tfList = db.TableFields.Where(tf => tf.TableID == field.ID).ToList();
                db.TableFields.RemoveRange(tfList);
            }
            db.Fields.Remove(field);
            db.SaveChanges();
            return RedirectToAction("Edit", "Form", new { id = field.FormID });
        }

        public ActionResult CreateFlag(int id)
        {
            return RedirectToAction("Create", "Flag", new { id = id });
        }

        public ActionResult ParentForm(int id)
        {
            return RedirectToAction("Edit", "Form", new { id = id });
        }
    }
}
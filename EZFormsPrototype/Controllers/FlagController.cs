using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.Utility;
using System.Data.Entity;
using EZFormsPrototype.ViewModels;
using System.Collections.ObjectModel;
using Microsoft.AspNet.Identity;

namespace EZFormsPrototype.Controllers
{
    public class FlagController : Controller
    {
        private EZFormsPrototype.DAL.FormContext db = new EZFormsPrototype.DAL.FormContext();
        // GET: Flag
        [Authorize]
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            return View(db.Flags.Where(f => f.userID == userID).ToList());
        }

        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,Name,Message,TriggerExpression,Level,FieldID,FormID")] Flag flag)
        {
            var userID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                flag.userID = userID;
                db.Flags.Add(flag);
                db.SaveChanges();
                return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
            }
            return RedirectToAction("Create", new { id = flag.FieldID });
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userID = User.Identity.GetUserId();

            Flag flag = db.Flags.Find(id);
            if(flag == null || flag.userID != userID)
            {
                return HttpNotFound();
            }
            ViewBag.Level = DropDownListUtility.GetFlagLevelDropdown(flag.Level);
            ViewBag.appearsOnSubmit = DropDownListUtility.GetFlagTriggerDropdown(flag.appearsOnSubmit);
            ViewBag.ExpressionBlocks = db.ExpressionBlocks.Where(e => e.FlagID == id).OrderBy(e => e.Order).ToList();
            List<FormField> fields = db.FormFields.Where(f => f.FormID == flag.FormID).ToList();
            ViewBag.Fields = fields;
            //send a hash map with table ID as key and list of fields as value
            Dictionary<int, List<TableField>> tableFields = new Dictionary<int, List<TableField>>();
            foreach(Field field in fields)
            {
                if (field.Type == "table")
                {
                    tableFields.Add(field.ID, db.TableFields.Where(f => f.TableID == field.ID).ToList());    
                }
            }
            ViewBag.TableFields = tableFields;
            List<int> x = new List<int>(tableFields.Keys);
            return View(flag);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Message,appearsOnSubmit,Level,FieldID,FormID,DependantFields,FlagConditions")] Flag flag)
        {
            var userID = User.Identity.GetUserId();
            string id = db.Flags.AsNoTracking().Where(f => f.ID == flag.ID).FirstOrDefault().userID;

            if (ModelState.IsValid && id == userID)
            {
                flag.userID = userID;
                db.Entry(flag).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
            }
            ViewBag.Level = DropDownListUtility.GetFlagLevelDropdown(flag.Level);
            return View(flag);
        }

        [Authorize]
        public ActionResult addBlock([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID,Order,DependantFieldID1,DependantFieldID2,CodeExpression, ViewExpression")] AddBlock data)
        {
            var userID = User.Identity.GetUserId();
            string id = db.Flags.AsNoTracking().Where(f => f.ID == data.ID).FirstOrDefault().userID;

            if(userID != id)
            {
                //this should never happen
                return RedirectToAction("Index", "Flag");
            }

            //save the flag data to the flag
            Flag flag = new Flag();
            flag.ID = data.ID;
            flag.Name = data.Name;
            flag.Message = data.Message;
            flag.Level = data.Level;
            flag.FieldID = data.FieldID;
            flag.FormID = data.FormID;
            flag.userID = userID;

            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();
            //create a block and save it
            ExpressionBlock block = new ExpressionBlock();
            block.Order = data.Order;
            block.DependantFieldID1 = data.DependantFieldID1;
            block.DependantFieldID2 = data.DependantFieldID2;
            block.CodeExpression = data.CodeExpression;
            block.ViewExpression = data.ViewExpression;
            block.FlagID = data.ID;

            ExpressionBlock collision = db.ExpressionBlocks.Where(e => e.Order == block.Order).FirstOrDefault();

            if(collision != null)
            {
                List<ExpressionBlock> blocks = db.ExpressionBlocks.OrderBy(x => x.Order).ToList();
                foreach (ExpressionBlock b in blocks)
                {
                    if (b.Order >= block.Order)
                    {
                        b.Order++;
                    }
                    db.Entry(b).State = EntityState.Modified;
                }
            }

            db.ExpressionBlocks.Add(block);
            db.SaveChanges();
            //return the edit view for the flag again
            return RedirectToAction("Edit", "Flag", new { id = data.ID });
        }

        [Authorize]
        public ActionResult RemoveBlock([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID,BlockToRemove")] RemoveBlock data)
        {
            var userID = User.Identity.GetUserId();
            string id = db.Flags.AsNoTracking().Where(f => f.ID == data.ID).FirstOrDefault().userID;

            if (userID != id)
            {
                //this should never happen
                return RedirectToAction("Index", "Flag");
            }

            //save the flag data to the flag
            Flag flag = new Flag();
            flag.ID = data.ID;
            flag.Name = data.Name;
            flag.Message = data.Message;
            flag.Level = data.Level;
            flag.FieldID = data.FieldID;
            flag.FormID = data.FormID;
            flag.userID = userID;

            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();

            ExpressionBlock block = db.ExpressionBlocks.Find(data.BlockToRemove);
            db.ExpressionBlocks.Remove(block);
            db.SaveChanges();
            return RedirectToAction("Edit", "Flag", new { id = data.ID });
        }

        [Authorize]
        public ActionResult Delete(int id)
        {
            var userID = User.Identity.GetUserId();
            Flag flag = db.Flags.Find(id);
            if(flag == null || flag.userID != userID)
            {
                return HttpNotFound();
            }
            return View(flag);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userID = User.Identity.GetUserId();
            Flag flag = db.Flags.Find(id);
            if(flag == null || flag.userID != userID)
            {
                return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
            }
            db.Flags.Remove(flag);
            db.SaveChanges();
            return RedirectToAction("Edit", "Field", new { id = flag.FieldID });
        }

        [Authorize]
        public ActionResult ParentField(int id)
        {
            return RedirectToAction("Edit", "Field", new {id = id});
        }
    }
}
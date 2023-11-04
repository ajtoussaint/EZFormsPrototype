using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EZFormsPrototype.Models;
using EZFormsPrototype.Utility;
using System.Data.Entity;
using EZFormsPrototype.ViewModels;

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
            ViewBag.ExpressionBlocks = db.ExpressionBlocks.Where(e => e.FlagID == id).OrderBy(e => e.Order).ToList();
            ViewBag.Fields = db.Fields.Where(f => f.FormID == flag.FormID).ToList();
            return View(flag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID,DependantFields,FlagConditions")] Flag flag)
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

        public ActionResult addBlock([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID,Order,DependantFieldID1,DependantFieldID2,CodeExpression, ViewExpression")] AddBlock data)
        {
            //save the flag data to the flag
            Flag flag = new Flag();
            flag.ID = data.ID;
            flag.Name = data.Name;
            flag.Message = data.Message;
            flag.Level = data.Level;
            flag.FieldID = data.FieldID;
            flag.FormID = data.FormID;

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

        public ActionResult RemoveBlock([Bind(Include = "ID,Name,Message,TriggerExpression,Level,FieldID,FormID,BlockToRemove")] RemoveBlock data)
        {
            //save the flag data to the flag
            Flag flag = new Flag();
            flag.ID = data.ID;
            flag.Name = data.Name;
            flag.Message = data.Message;
            flag.Level = data.Level;
            flag.FieldID = data.FieldID;
            flag.FormID = data.FormID;

            db.Entry(flag).State = EntityState.Modified;
            db.SaveChanges();

            ExpressionBlock block = db.ExpressionBlocks.Find(data.BlockToRemove);
            db.ExpressionBlocks.Remove(block);
            db.SaveChanges();
            return RedirectToAction("Edit", "Flag", new { id = data.ID });
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
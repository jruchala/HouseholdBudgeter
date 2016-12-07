using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budgeter.Models;
using Microsoft.AspNet.Identity;
using Budgeter.Helpers;

namespace Budgeter.Controllers
{
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var budgetId = db.Budgets.FirstOrDefault(b => b.HouseholdId == user.HouseholdId).Id;
            return RedirectToAction("Details", new { id = budgetId } );
        }

        // GET: Budgets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Amount,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        public bool UpdateBudgetAmount(bool AddAmount, decimal Amount, int Frequency, int? BudgetId)
        {
            var budget = db.Budgets.Find(BudgetId);
            budget.Amount = (AddAmount) ? budget.Amount + (Amount * Frequency / 12) : budget.Amount - (Amount * Frequency / 12);
            db.Entry(budget).State = EntityState.Modified;
            db.SaveChanges();
            return true; 
        }

        // GET: Budgets/AddBudgetItem/5
        public ActionResult AddBudgetItem(int? budgetId)
        {
            ViewBag.BudgetId = budgetId;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Budgets/AddBudgetItem/5
        [HttpPost]
        public ActionResult AddBudgetItem(int frequency, decimal amount, int categoryId)
        {
            if (ModelState.IsValid)
            {
                var householdId = User.Identity.GetHouseholdId();
                var budget = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId);
                var item = new BudgetItem();
                item.Frequency = frequency;
                item.Amount = amount;
                item.CategoryId = categoryId;
                item.Category = db.Categories.FirstOrDefault(c => c.Id == categoryId);
                db.BudgetItems.Add(item);
                budget.BudgetItems.Add(item);
                
                db.SaveChanges();
                UpdateBudgetAmount(true, amount, frequency, budget.Id);


                return RedirectToAction("Details", new { id = budget.Id });
            }

            return RedirectToAction("Index");
        }

        // GET: Budgets/EditBudgetItem/5
        public ActionResult EditBudgetItem(int? id)
        {
            var model = db.BudgetItems.Find(id);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", model.CategoryId);
            return View(model);
        }
        
        // POST: Budgets/EditBudgetItem/5
        [HttpPost]
        public ActionResult EditBudgetItem([Bind(Include = "Id,BudgetId,Amount,Frequency,CategoryId")] BudgetItem item)
        {
            if (ModelState.IsValid)
            {
                var householdId = User.Identity.GetHouseholdId();
                var budget = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId);
                var oldItem = db.BudgetItems.AsNoTracking().FirstOrDefault(m => m.Id == item.Id);

                budget.Amount -= oldItem.Amount * oldItem.Frequency / 12;
                budget.Amount += item.Amount * item.Frequency / 12;

                db.BudgetItems.Attach(item);
                db.Entry(item).Property("Amount").IsModified = true;
                db.Entry(item).Property("Frequency").IsModified = true;
                db.Entry(item).Property("CategoryId").IsModified = true;
                db.Budgets.Attach(budget);
                db.Entry(budget).Property("Amount").IsModified = true;
               
                db.SaveChanges();

            }

            return RedirectToAction("Index", "Budgets");
        }


        // GET: Budgets/DeletBudgetItem/5
        public ActionResult DeleteBudgetItem(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BudgetItem item = db.BudgetItems.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Budgets/DeleteBudgetItem/5
        [HttpPost]
        public ActionResult DeleteBudgetItem(int id)
        {
            var category = db.Categories.Find(id);
            var item = db.BudgetItems.Find(id);
            UpdateBudgetAmount(false, item.Amount, item.Frequency, item.BudgetId);
            db.BudgetItems.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Details", "Budgets", new { id = item.BudgetId });
        }

        // GET: Budgets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Amount,HouseholdId")] Budget budget)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Budget budget = db.Budgets.Find(id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Budget budget = db.Budgets.Find(id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
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

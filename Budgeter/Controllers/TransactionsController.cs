using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Budgeter.Models;
using Budgeter.Helpers;
using Microsoft.AspNet.Identity;

namespace Budgeter.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var householdId = User.Identity.GetHouseholdId();
            var transactions = db.Accounts.Where(a => a.HouseholdId == householdId);
            return View(transactions.ToList());
        }

        //POST: Transaction/AddAccount
        [HttpPost]
        public ActionResult AddAccount(string accountName)
        {
            Account account = new Account();
            account.Name = accountName;
            account.Balance = 0M;
            account.ReconciledBalance = 0M;
            var household = db.Households.Find(User.Identity.GetHouseholdId());
            household.Accounts.Add(account);
            db.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }



        // GET: Transactions/AddTransaction
        public ActionResult AddTransaction(int? id)
        {
            try
            {
                ViewBag.AccountId = id;
                var household = db.Households.Find(User.Identity.GetHouseholdId());
                var categories = household.Categories.Select(b => b.BudgetItems).Distinct().ToList();
                ViewBag.CategoryId = categories;
                return PartialView();
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        // POST: Transactions/AddTransaction
        [HttpPost]
        public ActionResult AddTransaction([Bind(Include = "Amount, Description, TransactionType")] Transaction transaction, int? accountId, int? budgetCategoryId)
        {
            if (ModelState.IsValid)
            {
                transaction.Date = DateTime.Now;
                var userId = User.Identity.GetUserId();
                transaction.EnteredById = userId;
                if (transaction.TransactionType == TransactionType.Expense)
                {
                    transaction.CategoryId = (int)budgetCategoryId;
                    UpdateAccountBalance(false, false, transaction.Amount, accountId);
                }
                else
                {
                    UpdateAccountBalance(true, false, transaction.Amount, accountId);
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var thisTransaction = db.Transactions.Find(transaction.Id);
                var account = db.Accounts.Find(accountId);
                account.Transactions.Add(thisTransaction);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = transaction.AccountId });

        }

        public bool UpdateAccountBalance(bool IsIncome, bool IsReconciled, decimal Amount, int? AccountId)
        {
            var account = db.Accounts.Find(AccountId);
            account.Balance = (IsIncome) ? account.Balance + Amount : account.Balance - Amount;
            if (IsReconciled)
            {
                account.ReconciledBalance = (IsIncome) ? account.ReconciledBalance + Amount : account.ReconciledBalance - Amount;
            }
            else
            {
                account.ReconciledBalance = account.ReconciledBalance;
            }
            db.Accounts.Attach(account);
            db.Entry(account).Property("Balance").IsModified = true;
            db.Entry(account).Property("ReconciledBalance").IsModified = true;
            db.SaveChanges();

            return true;
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Date,Amount,Type,Reconciled,ReconciledAmount,AccountId,CategoryId,EnteredById")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }



        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Date,Amount,Type,Reconciled,ReconciledAmount,AccountId,CategoryId,EnteredById")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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

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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.Category);
            return View(transactions.ToList());
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

        // GET: Transactions/Create
        public ActionResult Create(int accountId)
        {
            ViewBag.AccountId = accountId;
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
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

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Description,Amount,Reconciled,ReconciledAmount,AccountId,CategoryId,TransactionType")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.EnteredById = User.Identity.GetUserId();
                transaction.Date = DateTime.Now;
                transaction.IsVoid = false;
                if (transaction.TransactionType == TransactionType.Expense)
                {
                    
                    UpdateAccountBalance(false, false, transaction.Amount, transaction.AccountId);
                }
                else
                {
                    UpdateAccountBalance(true, false, transaction.Amount, transaction.AccountId);
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId });
            }

            ViewBag.AccountId = transaction.AccountId;
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
            var householdId = User.Identity.GetHouseholdId();
            var household = db.Households.Find(householdId);
            var accounts = household.Accounts.ToList();
            ViewBag.AccountId = new SelectList(accounts, "Id", "Name", transaction.AccountId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Date,Amount,Reconciled,ReconciledAmount,AccountId,CategoryId,TransactionType")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var originalTransaction = db.Transactions.AsNoTracking().FirstOrDefault(t => t.Id == transaction.Id);
                decimal AccountAmount = transaction.Amount - originalTransaction.Amount;
                if (transaction.TransactionType == TransactionType.Expense)
                {
                    UpdateAccountBalance(false, false, AccountAmount, transaction.AccountId);
                }
                else
                {
                    UpdateAccountBalance(true, false, AccountAmount, transaction.AccountId);
                }
                db.Transactions.Attach(transaction);
                db.Entry(transaction).Property("Amount").IsModified = true;
                db.Entry(transaction).Property("Description").IsModified = true;
                db.Entry(transaction).Property("Date").IsModified = true;
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Accounts", new { id = transaction.AccountId });
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
        public ActionResult DeleteConfirmed(int id, int AccountId)
        {
            Transaction transaction = db.Transactions.Find(id);
            bool AddBalance = (transaction.TransactionType == TransactionType.Expense) ? true : false;
            if (!transaction.IsVoid)
            {
                UpdateAccountBalance(AddBalance, false, transaction.Amount, transaction.AccountId);
            }
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Details", "Accounts", new { id = AccountId });
        }

        // GET: Transactions/Void/5
        public ActionResult Void(int? id)
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

        // POST: Transactions/Void/5
        [HttpPost, ActionName("Void")]
        [ValidateAntiForgeryToken]
        public ActionResult VoidConfirmed(int id, int AccountId)
        {
            Transaction transaction = db.Transactions.Find(id);
            bool AddBalance = (transaction.TransactionType == TransactionType.Expense) ? true : false;
            UpdateAccountBalance(AddBalance, false, transaction.Amount, transaction.AccountId);
            transaction.Description += " VOID";
            transaction.IsVoid = true;
            
            db.SaveChanges();
            return RedirectToAction("Details", "Accounts", new { id = AccountId });
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

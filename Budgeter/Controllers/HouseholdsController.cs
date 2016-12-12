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
using System.Threading.Tasks;
using Budgeter.Helpers;

namespace Budgeter.Controllers
{
    [RequireHttps]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BudgetItemsCreationHelper helper = new BudgetItemsCreationHelper();

        // GET: Households
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var householdId = user.HouseholdId;
            return RedirectToAction("Details", new { id = householdId });
        }

        // GET: Households/JoinHousehold
        public ActionResult JoinHousehold()
        {
            return View();
        }
           

        // POST: Households/JoinHousehold
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> JoinHousehold(string inviteCode)
        {
            var invite = db.Invitations.FirstOrDefault(i => i.Code == inviteCode);
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (invite != null && invite.Expired == false)
            {
                invite.Expired = true;
                db.Invitations.Attach(invite);
                db.Entry(invite).Property("Expired").IsModified = true;
                db.SaveChanges();
                var household = db.Households.Find(invite.HouseholdId);
                if (household != null)
                {
                    household.Users.Add(user);
                    db.SaveChanges();
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Message = "That invitation is invalid or has expired.";
            return View();
        }

        // POST: Households/LeaveHousehold/5
        [AuthorizeHousehold]
        [HttpPost]
        public async Task<ActionResult> LeaveHousehold(bool? confirmLeaveHousehold)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var householdId = User.Identity.GetHouseholdId();
            var household = db.Households.Find(householdId);

            if (confirmLeaveHousehold != null && household.Users.Contains(user))
            {
                household.Users.Remove(user);
                db.SaveChanges();
                await ControllerContext.HttpContext.RefreshAuthentication(user);
                return RedirectToAction("JoinHousehold");
            }

            return RedirectToAction("Index");
        }

        // GET: Households/Details/5
        [AuthorizeHousehold]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            ViewBag.HouseholdId = id;
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                household.Users.Add(user);
                Budget budget = new Budget();
                budget.HouseholdId = household.Id;
                budget.household = household;
                budget.Amount = 0M;
                budget.Name = "Budget for " + household.Name;
                
                db.Budgets.Add(budget);
                user.HouseholdId = household.Id;
                household.Budgets.Add(budget);
                db.Households.Add(household);


                db.SaveChanges();
                helper.CreateBudgetItems(budget.Id);
                db.SaveChanges();
                if (household != null)
                {
                    await ControllerContext.HttpContext.RefreshAuthentication(user);
                    return RedirectToAction("Details", new { id = household.Id });
                }
                return RedirectToAction("Index", "Home");

            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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

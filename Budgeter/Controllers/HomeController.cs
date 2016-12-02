using Budgeter.Helpers;
using Budgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Budgeter.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var householdId = User.Identity.GetHouseholdId();
            var model = db.Households.Find(householdId);
            ViewBag.AccountsCount = model.Accounts.Count();
            ViewBag.MemberCount = model.Users.Count();
            ViewBag.HouseholdName = model.Name;
            ViewBag.BudgetsCount = model.Budgets.Count();
            ViewBag.AccountAmount = model.Accounts.FirstOrDefault().Balance;
            ViewBag.AccountName = model.Accounts.FirstOrDefault().Name;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
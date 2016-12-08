using Budgeter.Helpers;
using Budgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Budgeter.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var householdId = User.Identity.GetHouseholdId();
                var model = db.Households.Find(householdId);
                if (householdId != null)
                {
                    ViewBag.AccountsCount = model.Accounts.Count();
                    ViewBag.MemberCount = model.Users.Count();
                    ViewBag.HouseholdName = model.Name;
                    ViewBag.BudgetsCount = model.Budgets.Count();
                    ViewBag.BudgetedExpenses = model.Budgets.FirstOrDefault().Amount;
                    
                    
                    if (model.Accounts.Any())
                    {
                        ViewBag.AccountAmount = "$" + model.Accounts.FirstOrDefault().Balance;
                        ViewBag.AccountName = "in " + model.Accounts.FirstOrDefault().Name;
                    }
                    else
                    {
                        ViewBag.AccountAmount = "Add a bank account to start tracking your budget";
                    }
                    return View(model);
                }
                else
                    return RedirectToAction("Create", "Households");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

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
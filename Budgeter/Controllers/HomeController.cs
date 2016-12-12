using Budgeter.Helpers;
using Budgeter.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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
        BudgetCalculationHelper helper = new BudgetCalculationHelper();

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

                    // begin chart info
                    
                    var budgetId = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId).Id;
                    var budgetItems = db.BudgetItems.Where(i => i.BudgetId == budgetId);
                    List<decimal> budgetItemAmount = new List<decimal>();
                    List<decimal> budgetItemExpense = new List<decimal>();
                    List<string> budgetItemName = new List<string>();

                    ViewBag.BudgetItemAmount = new decimal[budgetItems.Count()];
                    ViewBag.BudgetItemExpense = new decimal[budgetItems.Count()];
                    ViewBag.BudgetItemName = new string[budgetItems.Count()];

                    foreach (var item in budgetItems)
                    {
                        budgetItemAmount.Add((int)item.Amount);   
                        budgetItemExpense.Add((int)helper.TotalExpensePerBudgetItem(item.Id));
                        budgetItemName.Add((string)item.Name);
                    }


                    for (int i = 0; i < budgetItems.Count(); i++)
                    {
                        ViewBag.BudgetItemAmount[i] = budgetItemAmount[i];
                        ViewBag.BudgetItemExpense[i] = budgetItemExpense[i];
                        ViewBag.BudgetItemName[i] = budgetItemName[i];
                    }


                    //foreach (var item in budgetItems)
                    //{
                    //    budgetItemAmount.Add(item.Amount);
                    //    ViewBag.BudgetItemAmount.Append(item.Amount);
                    //    budgetItemExpense.Add(helper.TotalExpensePerBudgetItem(item.Id));
                    //    ViewBag.BudgetItemExpense.Append(helper.TotalExpensePerBudgetItem(item.Id));
                    //    budgetItemName.Add(item.Name);
                    //    ViewBag.BudgetItemName.Append(item.Name);
                    //}
                  



                    // end chart info


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
        
        public ActionResult GetChart()
        {

            var householdId = User.Identity.GetHouseholdId();
            var budgetId = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId).Id;
            var budgetItems = db.BudgetItems.Where(i => i.BudgetId == budgetId);
            List<decimal> budgetItemAmount = new List<decimal>();
            List<decimal> budgetItemExpense = new List<decimal>();
            List<string> budgetItemName = new List<string>();
            foreach (var item in budgetItems)
            {
                budgetItemAmount.Add(item.Amount);
                budgetItemExpense.Add(helper.TotalExpensePerBudgetItem(item.Id));
                budgetItemName.Add(item.Name);
            }


                var data = new[]
                {
                    new { y = budgetItemName[0], a = budgetItemAmount[0], b = budgetItemExpense[0]},
                    new { y = budgetItemName[1], a = budgetItemAmount[1], b = budgetItemExpense[1]},
                    new { y = budgetItemName[2], a = budgetItemAmount[2], b = budgetItemExpense[2]},
                    new { y = budgetItemName[3], a = budgetItemAmount[3], b = budgetItemExpense[3]},
                    new { y = budgetItemName[4], a = budgetItemAmount[4], b = budgetItemExpense[4]}
                };

                //return Content(JsonConvert.SerializeObject(data), "application/json");
                return Json(data, JsonRequestBehavior.AllowGet);

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
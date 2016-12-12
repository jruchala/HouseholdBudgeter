using Budgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budgeter.Helpers
{
    public class BudgetItemsCreationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void CreateBudgetItems(int budgetId)
        {
            var budget = db.Budgets.FirstOrDefault(b => b.Id == budgetId);

            //AddBudgetItem(12, 0M, "Housing");
            //AddBudgetItem(12, 0M, "Utilities");
            //AddBudgetItem(12, 0M, "Transportation");
            //AddBudgetItem(12, 0M, "Groceries");
            //AddBudgetItem(12, 0M, "Clothing");
            //AddBudgetItem(12, 0M, "Entertainment");
            BudgetItem housing = new BudgetItem();
            housing.BudgetId = budgetId;
            housing.Name = "Housing";
            housing.Amount = 0M;
            housing.Frequency = 12;
            housing.Budget = budget;
            db.BudgetItems.Add(housing);
            BudgetItem utilities = new BudgetItem();
            utilities.BudgetId = budgetId;
            utilities.Name = "Utilities";
            utilities.Amount = 0M;
            utilities.Frequency = 12;
            utilities.Budget = budget;
            db.BudgetItems.Add(utilities);
            BudgetItem transportation = new BudgetItem();
            transportation.BudgetId = budgetId;
            transportation.Name = "Transportation";
            transportation.Amount = 0M;
            transportation.Frequency = 12;
            transportation.Budget = budget;
            db.BudgetItems.Add(transportation);
            BudgetItem groceries = new BudgetItem();
            groceries.BudgetId = budgetId;
            groceries.Name = "Groceries";
            groceries.Amount = 0M;
            groceries.Frequency = 12;
            groceries.Budget = budget;
            db.BudgetItems.Add(groceries);
            BudgetItem clothing = new BudgetItem();
            clothing.BudgetId = budgetId;
            clothing.Name = "Clothing";
            clothing.Amount = 0M;
            clothing.Frequency = 12;
            clothing.Budget = budget;
            db.BudgetItems.Add(clothing);
            BudgetItem entertainment = new BudgetItem();
            entertainment.BudgetId = budgetId;
            entertainment.Name = "Entertainment";
            entertainment.Amount = 0M;
            entertainment.Frequency = 12;
            entertainment.Budget = budget;
            db.BudgetItems.Add(entertainment);
            BudgetItem income = new BudgetItem();
            income.BudgetId = budgetId;
            income.Name = "Income";
            income.Amount = 0M;
            income.Frequency = 12;
            income.Budget = budget;
            db.BudgetItems.Add(income);
            db.SaveChanges();

        }
    }
}
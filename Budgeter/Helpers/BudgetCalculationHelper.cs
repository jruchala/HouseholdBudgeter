using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Budgeter.Helpers;
using Budgeter;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;

namespace Budgeter.Helpers
{
    public class BudgetCalculationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        

        public decimal TotalExpensePerBudgetItem(int budgetItemId)
        {
            decimal totalExpense = 0M;
            var householdId = HttpContext.Current.User.Identity.GetHouseholdId();
            var accounts = db.Accounts.Where(a => a.HouseholdId == householdId);
            foreach (var account in accounts)
            {
                var transactions = db.Transactions.Where(t => t.AccountId == account.Id);
                foreach (var transaction in transactions)
                {
                    if (transaction.BudgetItemId == budgetItemId && transaction.TransactionType == TransactionType.Expense)
                    {
                        totalExpense += transaction.Amount;
                    }
                }
            }
            return totalExpense;
        }

        

        public decimal TotalExpenses(int householdId)
        {
            decimal totalExpenses = 0M;
            var accounts = db.Accounts.Where(a => a.HouseholdId == householdId);
            foreach (var account in accounts)
            {
                var transactions = db.Transactions.Where(t => t.AccountId == account.Id);
                foreach(var transaction in transactions)
                {
                    if (transaction.TransactionType == TransactionType.Expense && !transaction.IsVoid)
                    {
                        totalExpenses += transaction.Amount;
                    }
                }
            }
            return totalExpenses;
        }
    }
}
using Budgeter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budgeter.Helpers
{
    public class BudgetCalculationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //public decimal BudgetIdSum(int? budgetId)
        //{
        //    decimal transactionSum = 0M;
        //    var items = db.BudgetItems.Where(i => i.BudgetId == budgetId);
        //    foreach (var item in items)
        //    {
        //        var itemTransactions = db.Transactions.Where(t => t.BudgetItemId == item.BudgetItemId);
        //        foreach (var transaction in itemTransactions)
        //        {
        //            transactionSum += transaction.Amount;
        //        }
        //    }
            
        //    return transactionSum;
        //}



        public decimal TotalExpenses(int householdId)
        {
            decimal totalExpenses = 0m;
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
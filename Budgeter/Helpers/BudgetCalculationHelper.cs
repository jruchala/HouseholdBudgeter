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

        public decimal BudgetIdSum(int budgetId)
        {
            decimal transactionSum = 0M;
            var items = db.BudgetItems.Where(i => i.BudgetId == budgetId);
            foreach (var item in items)
            {
                var itemTransactions = db.Transactions.Where(t => t.CategoryId == item.CategoryId);
                foreach (var transaction in itemTransactions)
                {
                    transactionSum += transaction.Amount;
                }
            }
            
            return transactionSum;
        }

        //public decimal BudgetDifference()
        //{

        //    return difference;
        //}
    }
}
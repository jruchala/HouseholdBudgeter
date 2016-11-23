using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Household> Households { get; set; }

        public Category()
        {
            this.BudgetItems = new HashSet<BudgetItem>();
            this.Transactions = new HashSet<Transaction>();
            this.Households = new HashSet<Household>();
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class BudgetItem
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }
        public int Frequency { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Budget Budget { get; set; }
        public virtual Category Category { get; set; }

        public BudgetItem()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}
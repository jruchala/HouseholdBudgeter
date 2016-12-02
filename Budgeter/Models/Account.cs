using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Display(Name="Bank Account")]
        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Balance { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name="Reconciled Balance")]
        public decimal? ReconciledBalance { get; set; }

        public int HouseholdId { get; set; }
        public virtual Household Household { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}
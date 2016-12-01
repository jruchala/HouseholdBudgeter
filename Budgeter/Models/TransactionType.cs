using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Budgeter.Models
{
    public enum TransactionType
    {
        [Display(Name = "Expense")]
        Expense,
        [Display(Name = "Income")]
        Income
    }
}
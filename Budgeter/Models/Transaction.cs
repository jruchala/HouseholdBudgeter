using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Transaction
    {
        //TODO:Transaction model
        public int Id { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }

        public bool Reconciled { get; set; }

        [Display(Name ="Reconciled Amount")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? ReconciledAmount { get; set; }

        [Display(Name ="Account")]

        public int AccountId { get; set; }

        [Display(Name ="Category")]
        public int CategoryId { get; set; }

        [Display(Name ="Entered By")]
        public string EnteredById { get; set; }

        [Display(Name="Void")]
        public bool IsVoid { get; set; }

        public virtual Account Account { get; set; }

        [Display(Name="Type")]
        public virtual TransactionType TransactionType { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser EnteredByUser { get; set; }
    }
}
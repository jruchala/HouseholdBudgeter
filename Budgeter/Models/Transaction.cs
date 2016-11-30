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
        public DateTime Date { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public bool Reconciled { get; set; }
        public decimal ReconciledAmount { get; set; }
        public int AccountId { get; set; }
        public int CategoryId { get; set; }
        public string EnteredById { get; set; }
        
        public virtual Account Account { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser EnteredByUser { get; set; }
    }
}
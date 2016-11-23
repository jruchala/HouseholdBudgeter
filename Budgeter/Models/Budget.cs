using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Display(Name = "Budget")]
        public string Name { get; set; }

        public virtual Household household { get; set; }
        public int HouseholdId { get; set; }
    }
}




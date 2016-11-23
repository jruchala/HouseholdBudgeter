using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }

        public virtual Household household { get; set; }
        public int HouseholdId { get; set; }
    }
}
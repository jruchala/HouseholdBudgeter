using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Budgeter.Models
{
    public class Household
    {
        public int Id { get; set; }

        [Display(Name="Household")]
        public string Name { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public Household()
        {
            this.Budgets = new HashSet<Budget>();
            this.Categories = new HashSet<Category>();
            this.Accounts = new HashSet<Account>();
            this.Invitations = new HashSet<Invitation>();
            this.Users = new HashSet<ApplicationUser>();

        }
    }


}
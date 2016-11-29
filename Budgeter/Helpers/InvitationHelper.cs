using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Text;

namespace Budgeter.Helpers
{
	public class InvitationHelper
	{
        EmailService email = new EmailService();
        ApplicationDbContext db = new ApplicationDbContext();

        public async Task InviteToHousehold(string household, string userEmail)
        {
            var msg = new IdentityMessage();
            msg.Subject = "Invitation to HouseFin";
            msg.Destination = userEmail;
            msg.Body = "You are invited to join the household " + household;

            await email.SendAsync(msg);
        }
	}
}
using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;




namespace Budgeter.Helpers 
{
    public class InvitationHelper 
	{
        EmailService email = new EmailService();
        ApplicationDbContext db = new ApplicationDbContext();

        public async Task InviteToHousehold(string household, string userEmail, string code, string callbackUrl)
        {
            var msg = new IdentityMessage();
            msg.Subject = "Invitation to HouseFin";
            msg.Destination = userEmail;
            msg.Body = "You are invited to join the household " + household + ". Click <a href=\"" + callbackUrl + "\">here</a> and enter code: <strong>" + code + "</strong>";

            await email.SendAsync(msg);
        }


	}
}
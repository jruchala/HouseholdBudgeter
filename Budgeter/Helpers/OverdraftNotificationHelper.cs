using Budgeter.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;




namespace Budgeter.Helpers
{
    public class OverdraftNotificationHelper
    {
        EmailService email = new EmailService();
        

        public async Task SendNotification(string accountName, string userEmail)
        {
            var msg = new IdentityMessage();
            msg.Subject = "HouseFin Overdraft Notification";
            msg.Destination = userEmail;
            msg.Body = "You have posted an overdraft to the account " + accountName + " in your HouseFin budget.";
            await email.SendAsync(msg);
        }


    }
}
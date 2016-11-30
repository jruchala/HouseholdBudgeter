using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Data.Entity.Validation;

namespace Budgeter.Helpers
{
    public static class HouseholdHelper
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var householdClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == "HouseholdId");

            if (householdClaim != null)
            {
                return Int32.Parse(householdClaim.Value);

            }
            else
                return null;
        }

        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hid = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hid != null && !string.IsNullOrWhiteSpace(hid.Value));
        }
    }
}
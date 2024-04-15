using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using InventoryManagement.Models;
using Newtonsoft.Json;
using InventoryManagement.Common;
using Serilog;


namespace InventoryManagement
{
    public static class IdentityExtension
    {
        public static int GetCurrentUserID(this IIdentity identity)
        {
            try
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;

                Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                return Convert.ToInt32(claim.Value);

            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message);
                throw new SecurityException("Not Authorized");
            }
            
        }

        public static int GetCurrentUserRoleId(this IIdentity identity)
        {

            try
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                Claim claim = claimsIdentity.FindFirst(ClaimTypes.UserData);
                UserData userindof = JsonConvert.DeserializeObject<UserData>(claim.Value);
                return userindof.RoleID;

            }
            catch (Exception eError)
            {
                Log.Logger.Error(eError.Message);
                throw new SecurityException("Not Authorized");
            }
           

        }
        public static string GetRoleCode(this IIdentity identity)
        {
            try
            {
                ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
                Claim claim = claimsIdentity.FindFirst(ClaimTypes.Role);
                return claim.Value;
            }
            catch (Exception eError)
            {
                Log.Logger.Error(eError.Message);
                throw new SecurityException("Not Authorized");
            }

            
        }




    }
}

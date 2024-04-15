using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Business.Common.Interface
{
    public interface ITokenServices
    {
        #region Interface member methods.
        /// <summary>
        ///  Function to generate unique token with expiry against the provided empId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        AppTokenDto GenerateToken(int empId);

        /// <summary>
        /// Function to validate token againt expiry and existance in database.
        /// </summary>
        /// <param name="tokenkey"></param>
        /// <returns></returns>
        AppTokenDto ValidateToken(string tokenkey);

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenkey"></param>
        bool Kill(string tokenkey);

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        bool DeleteByUserId(int empId);





        IdentityUser SignIn(string username);

        IdentityUser SignIn(string username, string password, int role);

        IdentityUser SignUp(string username, string password);
        bool SignOut(string TokenKey);

        bool ValidatePassword(int userid, string password);

        #endregion

    }
}

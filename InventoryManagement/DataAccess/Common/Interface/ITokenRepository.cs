using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Common.Interface
{
    public interface ITokenRepository
    {
        IdentityUser SignIn(string username);
        IdentityUser SignUp(string username,string password);
        AppTokenDto GenerateToken(int userId);

        bool SignOut(string TokenKey);

        AppTokenDto ValidateToken (string TokenKey);

        int InvalidateOldTokensOfUser(int empId);

        bool ValidatePassword(int userId, string password);

        bool RenewToken(string TokenId);
    }
}

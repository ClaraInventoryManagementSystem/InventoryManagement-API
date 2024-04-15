

using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Elab.Interface
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsersList();
        User GetUserByID(int UserId);
        Task<User> GetUserByIDAsync(int UserId);
        IEnumerable<User> GetUsersListByRole(int RoleId);

        IEnumerable<User> GetUsersListByRole(int[] RoleIdList);

        int CreateUser(User user);
        bool UpdateUserInfo(User user);

        bool UpdateUserStatus(int status, int userId);
        bool UpdateUserPassword(User user);
    }
}

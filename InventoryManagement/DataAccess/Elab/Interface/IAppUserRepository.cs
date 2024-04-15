using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Elab.Interface
{
    public interface IAppUserRepository
    {
        IEnumerable<KeyValue> GetRoles();
        AppUser GetUserByID(int UserId);
        Task<User> GetUserByIDAsync(int UserId);
        IEnumerable<AppUser> GetUsersListByRole(int RoleId);

        IEnumerable<AppUser> GetUsersListByRole(int[] RoleIdList);
    }
}

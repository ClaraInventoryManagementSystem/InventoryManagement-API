using InventoryManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Business.Elab.Interface
{
    public interface IAppUserBusiness
    {
       Task<User> GetEmployeeByID(int UserId);
    }
}

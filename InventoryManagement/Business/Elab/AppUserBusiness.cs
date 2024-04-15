using InventoryManagement.Business.Elab.Interface;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Business.Elab
{
    public class AppUserBusiness: IAppUserBusiness
    {
        private readonly IConfiguration _config;
        RepositoryCreator objRepo;

        public AppUserBusiness(IConfiguration config)
        {
            _config = config;
            objRepo = new RepositoryCreator(_config);
        }

        public async Task<User> GetEmployeeByID(int UserId)
        {
             return  await objRepo.AppUserRepository.GetUserByIDAsync(UserId);
            //throw new NotImplementedException();
        }
    }
}

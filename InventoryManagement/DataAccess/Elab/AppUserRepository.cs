using InventoryManagement.DataAccess.Elab.Interface;
using InventoryManagement.Models;
using System;
using Dapper;
using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Common;
using Serilog;

namespace InventoryManagement.DataAccess.Elab
{
    public class AppUserRepository : DataAccessRepository<AppUser>, IAppUserRepository
    {
        private IConfiguration _config { get; set; }
        public AppUserRepository(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public IEnumerable<KeyValue> GetRoles()
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                return GetRecord<KeyValue>(DBQueryConstant.GET_ROLES, p, CommandType.Text);
            }
            catch (Exception eError)
            {
                Log.Logger.Error(eError.Message);
                throw eError;
            }


        }


        public AppUser GetUserByID(int UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return GetRecordById<AppUser>(DBQueryConstant.GetUserByID, param, CommandType.Text);
        }
        public async Task<User> GetUserByIDAsync(int UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return await GetRecordByIdAsync<User>(DBQueryConstant.GetUserByID, param, CommandType.Text);
        }
        public IEnumerable<AppUser> GetUsersListByRole(int RoleId)
        {
            var param = new DynamicParameters();
            param.Add("@RoleId", RoleId);
            return GetRecord<AppUser>(DBQueryConstant.GetUserListByRole, param, CommandType.Text);
        }

        public IEnumerable<AppUser> GetUsersListByRole(int[] RoleIdList)
        {
            string RoleList = String.Empty;
            foreach (int role in RoleIdList)
                RoleList += role.ToString() +",";

            RoleList =  RoleList.TrimEnd(',');

            var param = new DynamicParameters();
            param.Add("@roleList", RoleList);
            return GetRecord<AppUser>(DBQueryConstant.GetUserListByRole_list, param, CommandType.Text);
        }
       
   
    }
}
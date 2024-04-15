
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
using InventoryManagement.Models.ViewModel;
using MySqlX.XDevAPI.Common;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.DataAccess.Elab
{
    public class UserRepository : DataAccessRepository<User>, IUserRepository
    {
        private IConfiguration _config { get; set; }
        public UserRepository(IConfiguration config) : base(config)
        {
            _config = config;
        }

        public IEnumerable<User> GetUsersList()
        {
            var param = new DynamicParameters();
            //param.Add("@RoleId", RoleId);
            return GetRecord<User>(DBQueryConstant.GetUserList, param, CommandType.Text);
        }

        public int CreateUser(User user)
        {
            //id, firstname, lastname, login, password, role, lastlogin, active
            int userid = 0;
            //firstname, @lastname, @login, @password, @role, @lastlogin, @active
            var p = new DynamicParameters();
            p.Add("@firstname", user.FirstName);
            p.Add("@lastname", user.LastName);
            p.Add("@login", user.LogIn);
            p.Add("@password", user.Password);
            p.Add("@role", user.Role);
            p.Add("@lastlogin", DateTime.Now);
            p.Add("@active", 1);            
           // p.Add("@ID", null, DbType.Int32, ParameterDirection.Output);
            userid = GetRecordById<int>(DBQueryConstant.CreateUser, p, CommandType.Text, null);
            return userid;
        }

        public bool UpdateUserInfo(User user)
        {
            bool bresult = false;
            int result = 0;            
            var p = new DynamicParameters();
            p.Add("@firstname", user.FirstName);
            p.Add("@lastname", user.LastName);
            p.Add("@password", user.Password);             
            p.Add("@UserID", user.Id);
            result = UpdateRecord(DBQueryConstant.UpdateUserInfo,ref p, CommandType.Text);
            if (result == 1)
                bresult = true;
            return bresult;
        }

        public bool UpdateUserStatus(int status, int userId)
        {
            bool bresult = false;
            int result = 0;
            var p = new DynamicParameters();
            p.Add("@active", status);            
            p.Add("@UserID", userId);
            result = UpdateRecord(DBQueryConstant.UpdateUserStatus, ref p, CommandType.Text);
            if (result == 1)
                bresult = true;
            return bresult;
        }

        public bool UpdateUserPassword(User user)
        {
            bool bresult = false;
            int result = 0;
            var p = new DynamicParameters();
            p.Add("@password", user.Password);            
            p.Add("@UserID", user.Id);
            result = UpdateRecord(DBQueryConstant.UpdateUserPassword, ref p, CommandType.Text);
            if (result == 1)
                bresult = true;
            return bresult;
        }

        public User GetUserByID(int UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return GetRecordById<User>(DBQueryConstant.GetUserByID, param, CommandType.Text);
        }

        public async Task<User> GetUserByIDAsync(int UserId)
        {
            var param = new DynamicParameters();
            param.Add("@UserId", UserId);
            return await GetRecordByIdAsync<User>(DBQueryConstant.GetUserByID, param, CommandType.Text);
        }

        public IEnumerable<User> GetUsersListByRole(int RoleId)
        {
            var param = new DynamicParameters();
            param.Add("@RoleId", RoleId);
            return GetRecord<User>(DBQueryConstant.GetUserListByRole, param, CommandType.Text);
        }

        public IEnumerable<User> GetUsersListByRole(int[] RoleIdList)
        {
            string RoleList = String.Empty;
            foreach (int role in RoleIdList)
                RoleList += role.ToString() + ",";

            RoleList = RoleList.TrimEnd(',');

            var param = new DynamicParameters();
            param.Add("@roleList", RoleList);
            return GetRecord<User>(DBQueryConstant.GetUserListByRole_list, param, CommandType.Text);
        }
    }
}

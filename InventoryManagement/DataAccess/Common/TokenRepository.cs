using InventoryManagement.Common;
using InventoryManagement.DataAccess.Common.Interface;
using InventoryManagement.Models;
//using InventoryManagement.Models.HR;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace InventoryManagement.DataAccess.Common
{
    public class TokenRepository : DataAccessRepository<AppTokenDto>, ITokenRepository
    {
        private  IConfiguration _config { get; set; }      
        public TokenRepository(IConfiguration config) : base(config)
        {
            _config = config;            
        }

        public AppTokenDto GenerateToken(int userId)
        {
            string tokenExpiryMinutes = "";
            if (_config != null && _config.GetSection("AppSettings:AuthTokenExpiry") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:AuthTokenExpiry").Value))
                tokenExpiryMinutes = _config.GetSection("AppSettings:AuthTokenExpiry").Value;
            else
                tokenExpiryMinutes = "60";
            string strTokenID = Guid.NewGuid().ToString();
            var param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@IssuedOn", DateTime.Now);
            param.Add("@AuthToken", strTokenID);
            param.Add("@ExpiresOn", DateTime.Now.AddMinutes(Convert.ToInt32(tokenExpiryMinutes)));
            param.Add("@IsActive", 1);            
            var result = CreateRecord(DBQueryConstant.GenerateToken, ref param, CommandType.Text);
            if (result == 1)
            {
                var param1 = new DynamicParameters();
                param1.Add("@UserId", userId);
                param1.Add("@IsActive", 1);
                AppTokenDto data =  GetRecord<AppTokenDto>(DBQueryConstant.GetTokenInfoByEMPID, param1, CommandType.Text).FirstOrDefault();
                if( data != null && data.AuthToken != strTokenID)
                {
                    Log.Logger.Error("Auth token is not macthing from database error*********************");
                    if (data != null && data.AuthToken != strTokenID)
                        Log.Logger.Error("token_inserted={0} token_retrund={1} userid={2}", strTokenID, data.AuthToken, data.User_Id);

                }
                return data;
            }

            return null;
        }

        public IdentityUser SignIn(string username)
        {
            var param = new DynamicParameters();
            IdentityUser identity = null;
            param.Add("@login", username);
           var empInfo = GetRecordById<User>(DBQueryConstant.UserDetailsByUserID, param, CommandType.Text);
            if (empInfo != null && empInfo.Id > 0)
            {
                //empInfo.ENCRYPTED_PASSWORD = CryptoManager.DecryptString(empInfo.ENCRYPTED_PASSWORD);
                identity = new IdentityUser();
                identity.userDetails = empInfo;
            }
            return identity;            
        }

        public bool SignOut(string TokenKey)
        {
            bool bresult = false;
            var param1 = new DynamicParameters();
            param1.Add("@AuthToken", TokenKey);
            param1.Add("@IsActive", 0);   
            var result = UpdateRecord(DBQueryConstant.UpdateTokenByAuthTokenKey, ref param1, CommandType.Text);
            if (result == 1)
                 bresult = true;

                return bresult;            
        }

        public AppTokenDto ValidateToken(string TokenKey)
        {
            int autoRenewTokenValue = 20;
            if (_config != null && _config.GetSection("AppSettings:AutoRenewTokenValue") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:AutoRenewTokenValue").Value))
                if (!int.TryParse(_config.GetSection("AppSettings:AutoRenewTokenValue").Value, out autoRenewTokenValue))
                    autoRenewTokenValue = 20;

            var param1 = new DynamicParameters();
            param1.Add("@AuthToken", TokenKey);
            param1.Add("@IsActive", 1);
            var tokenEntity = GetRecord<AppTokenDto>(DBQueryConstant.ValidateTokenKEy, param1, CommandType.Text).FirstOrDefault();
            if (tokenEntity != null && tokenEntity.User_Id > 0)
            {
                DateTime currentDate = DateTime.Now;
                if (tokenEntity.Expired_On > currentDate)
                {
                    
                    if ( (tokenEntity.Expired_On - currentDate).TotalSeconds <= autoRenewTokenValue)
                    {
                        TokenRenewer.RenewToken(TokenKey);
                    }
                    return tokenEntity;
                }
                    
                else
                {
                    Log.Error(string.Format("*** token expired signout is calling tk={0} texp={1} nw={2}",
                         TokenKey,
                         tokenEntity.Expired_On.ToString(),
                         currentDate.ToString()));
                    // To Inactive token.
                   var result = SignOut(TokenKey);
                }
            }
            Log.Error("** failed to auth return null" + TokenKey);
            return null;
        }

        public int InvalidateOldTokensOfUser(int empId)
        {
            var param = new DynamicParameters();
            param.Add("@User_Id", empId);
            return UpdateRecord(DBQueryConstant.InvalidateOldTokensOfUser, ref param, CommandType.Text);

        }

        public IdentityUser SignUp(string username, string password)
        {
            var param = new DynamicParameters();
            IdentityUser identity = null;
            param.Add("@UserId", username);
            param.Add("@Password", password);
            var result = CreateRecord(DBQueryConstant.AddUser, ref param, CommandType.Text);
            if (result == 1)
            {
                var param1 = new DynamicParameters();
                param.Add("@UserId", username);
                var empInfo = GetRecordById<User>(DBQueryConstant.UserDetailsByUserID, param1, CommandType.Text);
                if (empInfo != null && empInfo.Id > 0)
                {
                    identity = new IdentityUser();
                    identity.userDetails = empInfo;
                }
            }

            return identity;

        }

        public bool ValidatePassword(int userId, string password)
        {

            var param = new DynamicParameters();
            param.Add("@UserID", userId);
           
            // 
            // To Decrypt the password
            var empInfo = GetRecordById<AppUser>(DBQueryConstant.ValidatePassword, param, CommandType.Text);
            if (empInfo != null && empInfo.User_Id > 0)
            {
                if (empInfo.User_Id == userId &&
                    CryptoManager.DecryptString(empInfo.ENCRYPTED_PASSWORD) == password )
                    return true;
            }
            return false;

        }


        public bool RenewToken(string TokenId)
        {
            try
            {
                string tokenExpiryMinutes = "";
                if (_config != null && _config.GetSection("AppSettings:AuthTokenExpiry") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:AuthTokenExpiry").Value))
                    tokenExpiryMinutes = _config.GetSection("AppSettings:AuthTokenExpiry").Value;
                else
                    tokenExpiryMinutes = "2";

                var param = new DynamicParameters();
                param.Add("@authtoken", TokenId);
                param.Add("@expired_on", DateTime.Now.AddMinutes(Convert.ToInt32(tokenExpiryMinutes)));
                UpdateRecord(DBQueryConstant.RENEW_USER_TOKEN, ref param, CommandType.Text);
                return true;

            }
            catch ( Exception eError)
            {
                throw eError;
            }
        }



    }
}

using InventoryManagement.Business.Common.Interface;
//using InventoryManagement.Business.HR;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Business.Common
{
    public class TokenServices : ITokenServices
    {
        #region Private member variables.        
        private readonly IConfiguration _config;
        RepositoryCreator objRepo;
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public TokenServices(IConfiguration config)
        {
            _config = config;
            objRepo = new RepositoryCreator(_config);

        }
        #endregion


        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AppTokenDto GenerateToken(int userId)
        {
            return objRepo.TokenRepository.GenerateToken(userId);            
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenKey"></param>
        /// <returns></returns>
        public AppTokenDto ValidateToken(string tokenKey)
        {            
            return objRepo.TokenRepository.ValidateToken(tokenKey);
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenKGenerateTokeney">true for successful delete</param>
        public bool Kill(string tokenKey)
        {            
            return objRepo.TokenRepository.SignOut(tokenKey);
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            //_unitOfWork.TokenRepository.Delete(x => x.UserId == userId);
            //_unitOfWork.Save();

            //var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.UserId == userId).Any();
            //return !isNotDeleted;
            return false;
        }

        public IdentityUser SignIn(string username)
        {
            return objRepo.TokenRepository.SignIn(username);
        }

        public IdentityUser SignIn(string username, string password, int role)
        {
            IdentityUser userInfo = null;
            userInfo = objRepo.TokenRepository.SignIn(username);
            // CHe            
            if (userInfo != null && userInfo.userDetails != null && !string.IsNullOrEmpty(userInfo.userDetails.LogIn) && (userInfo.userDetails.Password == password) && (userInfo.userDetails.roleid == role))
            {
                int kill_old_sessions = objRepo.TokenRepository.InvalidateOldTokensOfUser(userInfo.userDetails.Id);

                var TokenEnt = GenerateToken(userInfo.userDetails.Id);
                if (TokenEnt != null && TokenEnt.Token_ID > 0)
                {
                    userInfo.AuthToken = TokenEnt.AuthToken;
                    userInfo.ExpiresOn = TokenEnt.Expired_On;
                    userInfo.userDetails.ENCRYPTED_PASSWORD = null;
                }
            }
            return userInfo;
        }
       

        public bool SignOut(string TokenKey)
        {
            return objRepo.TokenRepository.SignOut(TokenKey);
        }

        public IdentityUser SignUp(string username, string password)
        {
            IdentityUser userInfo = null;
            userInfo = objRepo.TokenRepository.SignUp(username,password);


            //TODO: Encript password with auto generated sucerity key
            if (userInfo != null && userInfo.userDetails != null && !string.IsNullOrEmpty(userInfo.userDetails.ENCRYPTED_PASSWORD) && userInfo.userDetails.ENCRYPTED_PASSWORD == password)
            {
                int kill_old_sessions = objRepo.TokenRepository.InvalidateOldTokensOfUser(userInfo.userDetails.Id);

                var TokenEnt = GenerateToken(userInfo.userDetails.Id);
                if (TokenEnt != null && TokenEnt.Token_ID > 0)
                {
                    userInfo.AuthToken = TokenEnt.AuthToken;
                    userInfo.ExpiresOn = TokenEnt.Expired_On;
                }
            }
            return userInfo;
        }

        public bool ValidatePassword(int userid, string password)
        {
            return objRepo.TokenRepository.ValidatePassword(userid, password);

        }



        #endregion
    }
}

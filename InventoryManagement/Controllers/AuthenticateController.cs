using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using InventoryManagement.Business.Common.Interface;
using InventoryManagement.Business.Common;
using InventoryManagement.Common;
using InventoryManagement.Handlers;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBaseEx
    {
        private  ITokenServices _tokenServices;
        private  IConfiguration _config;
        IOptionsMonitor<AuthenticationSchemeOptions> _options;
        private readonly ILogger<AuthenticateController> _logger;

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AuthenticateController(ITokenServices tokenServices, IConfiguration config, IOptionsMonitor<AuthenticationSchemeOptions> options, ILogger<AuthenticateController> logger)
        {
            _tokenServices = tokenServices;
            _config = config;
            _options = options;
            _logger = logger;
        }

       

        [HttpPost]
        [Route("ValidatePassword")]
        public ActionResult ValidatePassword([FromHeader(Name = "Password")] string password)
        {
            ApiResponseWithStatus res = new ApiResponseWithStatus();
            try
            {  
                if (_tokenServices == null)
                    _tokenServices = new TokenServices(_config);

                bool bStatus = _tokenServices.ValidatePassword(User.Identity.GetCurrentUserID(),
                    password);
                res.Status = bStatus;
                if( bStatus == false)
                {
                    SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.PasswordValidationFailed,
                        User.Identity.GetCurrentUserID(),
                        User.Identity.GetCurrentUserRoleId(),
                        "Invalid Password");
                    res.ErrorMessage = "Invalid Password";
                }
               
            }
            catch( Exception eError)
            {
                res.Status = false;
                res.ErrorMessage = "Unexpected error";
                _logger.LogError(eError.Message);
            }

            return Ok(res);

        }

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>        
        [AllowAnonymous]
        [HttpPost]
        [Route("SignIn")]
        public ActionResult SignIn([FromHeader] string username, [FromHeader] string password, [FromHeader] int role)
        {
            _logger.LogInformation("Sign start");
            _logger.LogInformation("SignIn usernam={0}", username);

            try
            {
                if (_tokenServices == null)
                    _tokenServices = new TokenServices(_config);

                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    _logger.LogInformation("Signin bad Request");
                    _logger.LogInformation("SignIn:Username or password cant be empty");
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                if ((Request.Headers.ContainsKey("username") && !string.IsNullOrEmpty(Request.Headers["username"].ToString()))
                     && (Request.Headers.ContainsKey("password") && string.IsNullOrEmpty(Request.Headers["password"].ToString())))
                {
                    username = Request.Headers["username"].ToString().Trim();
                    password = Request.Headers["password"].ToString().Trim();
                }
                
                var userInfo = _tokenServices.SignIn(username, password, role);
                if (userInfo != null && userInfo.userDetails != null && !string.IsNullOrEmpty(userInfo.AuthToken))
                {
                    Response.Headers.Add(WebHeaders.TokenKey, userInfo.AuthToken);
                    _logger.LogInformation("Sending token in tok={0}", userInfo.AuthToken);
                    SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.LoginSuccess,
                        userInfo.userDetails.Id,
                        userInfo.userDetails.roleid,
                        this.RemoteIPAddress);
                    return Ok(userInfo);
                }
                else
                {
                    _logger.LogInformation("bad Request");
                    SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.LoginFailed,
                        0,
                        0,
                        username,
                        "Invalid user name or password Or Role",
                        this.RemoteIPAddress);
                    return BadRequest(new { message = "Username or password Or Role is incorrect" });
                }
            }
            catch( Exception eError)
            {
                _logger.LogError("Exception in Signin ");
                _logger.LogError(eError.Message);
                return BadRequest(new { message = "Username or password Or Role is incorrect" });
            }                        
        }


        //[AllowAnonymous]
        //[HttpPost]
        //[Route("SignIn2")]
        //public ActionResult SignIn2([FromHeader] string username, [FromHeader] string password, [FromHeader] int role)
        //{
        //    _logger.LogInformation("Sign start");
        //    _logger.LogInformation("SignIn usernam={0}", username);

        //    try
        //    {
        //        if (_tokenServices == null)
        //            _tokenServices = new TokenServices(_config);

        //        if (string.IsNullOrEmpty(username))
        //        {
        //            return BadRequest(new { message = "Invalid Username" });
        //        }
        //        if (string.IsNullOrEmpty(password))
        //        {
        //            return BadRequest(new { message = "Invalid password" });
        //        }
        //        if (role <= 0)
        //        {
        //            return BadRequest(new { message = "Invalid role" });

        //        }
        //        string ErrorMssage = String.Empty;
        //        bool bIsAccountLocked = false;

        //        var userInfo = _tokenServices.SignIn(username, password, role, out ErrorMssage, out bIsAccountLocked);
        //        if (userInfo != null && userInfo.userDetails != null && !string.IsNullOrEmpty(userInfo.AuthToken))
        //        {
        //            Response.Headers.Add(WebHeaders.TokenKey, userInfo.AuthToken);
        //            _logger.LogInformation("Sending token in tok={0}", userInfo.AuthToken);
        //            SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.LoginSuccess,
        //                userInfo.userDetails.User_Id,
        //                userInfo.userDetails.ROLE_ID,
        //                this.RemoteIPAddress);
        //            //return Ok(new LoginResponse(userInfo));
        //            return Ok(userInfo);
        //        }
        //        else
        //        {
        //            _logger.LogInformation("bad Request");
        //            SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.LoginFailed,
        //                0,
        //                0,
        //                username,
        //                "Invalid user name or password",
        //                this.RemoteIPAddress);
        //            if (bIsAccountLocked)
        //            {
        //                SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.LoginFailed,
        //                0,
        //                0,
        //                username,
        //                "Account Locked Due to Max Failed Login Attempts",
        //                this.RemoteIPAddress);
        //            }
        //            return Unauthorized(new { message = ErrorMssage });
        //        }
        //    }
        //    catch (Exception eError)
        //    {
        //        _logger.LogError("Exception in Signin ");
        //        _logger.LogError(eError.Message);
        //        return BadRequest(new { message = "Username or password is incorrect" });
        //    }
        //}

        /// <summary>
        /// SignOut user and returns token with expiry.
        /// </summary>
        /// <returns>bool</returns>     
        [AllowAnonymous]
        [HttpGet]
        [Route("SignOut")]
        public ActionResult SignOut()
        {
            string userToken = Request.Headers[WebHeaders.TokenKey].ToString();
            if (string.IsNullOrEmpty(userToken))
            {
                _logger.LogInformation("SignOut: Invalid Token");
                return BadRequest(new { message = "Token is invalid" });
            }
            SystemAuditLog.ProcessSystemAuditLog(AuditLogAction.Logout,
                        User.Identity.GetCurrentUserID(),
                        User.Identity.GetCurrentUserRoleId(),
                        this.RemoteIPAddress);

            return Ok(_tokenServices.SignOut(userToken));
        }

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>        
        [AllowAnonymous]
        [HttpPost]
        [Route("SignUp")]
        public ActionResult SignUp()
        {
            if (_tokenServices == null)
                _tokenServices = new TokenServices(_config);

            string username = string.Empty;
            string password = string.Empty;
            int orleId = 0;

            if ((!Request.Headers.ContainsKey("username") || string.IsNullOrEmpty(Request.Headers["username"].ToString()))
                                || (!Request.Headers.ContainsKey("password") || string.IsNullOrEmpty(Request.Headers["password"].ToString())))
            {
                _logger.LogInformation("SignIn:Username or password is incorrect");
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            username = Request.Headers["username"].ToString().Trim();

            password = Request.Headers["password"].ToString().Trim();

            var userInfo = _tokenServices.SignIn(username, password, orleId);
            if (userInfo != null && userInfo.userDetails != null && !string.IsNullOrEmpty(userInfo.AuthToken))
            {
                Response.Headers.Add(WebHeaders.TokenKey, userInfo.AuthToken);
                return Ok(userInfo);
            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });

            }
        }
    }
}

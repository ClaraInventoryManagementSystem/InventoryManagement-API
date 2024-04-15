using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using InventoryManagement.Business.Common.Interface;
using InventoryManagement.DataAccess.Common.Interface;
using InventoryManagement.Models;
//using InventoryManagement.Models.HR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using InventoryManagement.Common;
using InventoryManagement.Business.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;



namespace InventoryManagement.Handlers
{
    public class InventoryAuthendicationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ITokenServices _userService;
        private readonly IConfiguration _config;
        private readonly ILogger<InventoryAuthendicationHandler> _logger;
        public InventoryAuthendicationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            ITokenServices userService,
            IConfiguration config,
            ILogger<InventoryAuthendicationHandler> ilogger)
            : base(options, logger, encoder, clock)
        {
            _userService = userService;
            _config = config;
            _logger = ilogger;


        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("TokenKey"))
            {
                _logger.LogError("HandleAuthenticateAsync: Authorization Failed no Token data in Request Header");
                _logger.LogError("request path ={0}", Request.Path);
                return AuthenticateResult.Fail(ErrorList.AuthorizationFailed.ToString()); // "Missing Authorization Header"
            }

            User user = null;
            BusinessRepositoryCreator objEmp = null;
            UserData UserInfo = null;
            try
            {
                var authHeader = Request.Headers["TokenKey"].ToString();
                if (!string.IsNullOrEmpty(authHeader))
                {
                    var tokenEnty = _userService.ValidateToken(authHeader);
                    if (tokenEnty != null && tokenEnty.User_Id > 0)
                    {
                        objEmp = new BusinessRepositoryCreator(_config);

                        user = await objEmp.AppUserBusiness.GetEmployeeByID(tokenEnty.User_Id);


                        if (user != null && user.Id > 0)
                        {
                            UserInfo = new UserData();
                            UserInfo.AppUser = user;
                            UserInfo.AuthToken = tokenEnty.AuthToken;
                            UserInfo.TokenExpiresOn = tokenEnty.Expired_On;
                            UserInfo.TokenID = tokenEnty.Token_ID;
                            UserInfo.UserID = user.Id;
                            UserInfo.RoleID = user.roleid;


                            var claims = new[] {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.FirstName + user.LastName),
                            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(UserInfo)),
                            new Claim(ClaimTypes.Role, user.Role),

                            };
                            var identity = new ClaimsIdentity(claims, Scheme.Name);
                            var principal = new ClaimsPrincipal(identity);
                            var ticket = new AuthenticationTicket(principal, Scheme.Name);
                            return AuthenticateResult.Success(ticket);
                        }
                    }
                }
                else
                { 

                    _logger.LogError("Invalid token key auth failed");
                    return AuthenticateResult.Fail("Invalid TokenKey");
                }
            }
            catch ( Exception eError)
            {
                _logger.LogError("HandleAuthenticateAsync=" + eError.Message);
                return AuthenticateResult.Fail("Invalid Authorization Key");
            }
            return AuthenticateResult.Fail("Invalid TokenKey");            
        }
    }

    public interface IUserService
    {
        Task<AppUser> Authenticate(string username, string password);
        Task<IEnumerable<AppUser>> GetAll();
    }    
}

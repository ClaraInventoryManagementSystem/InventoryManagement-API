using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Controllers.Elab;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBaseEx
    {
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _config;
        RepositoryCreator objRepo;

        public UserController(IConfiguration config, ILogger<UserController> logger)
        {
            _config = config;
            _logger = logger;
            //objBusinessCreator = new BusinessRepositoryCreator(_config);
            objRepo = new RepositoryCreator(_config);
        }


        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            try
            {                
                if(user == null)
                    return BadRequest(new { message = "User Data Missing" });

                var result = objRepo.UserRepository.CreateUser(user);
                return Ok("User Created Sucessfully.");

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }

        [HttpPost]
        [Route("UpdateUserInfo")]
        public IActionResult UpdateUserInfo([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { message = "User Data Missing" });

                var result = objRepo.UserRepository.UpdateUserInfo(user);
                return Ok("User Details Updated Sucessfully.");

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }

        [HttpPost]
        [Route("UpdateUserStatus")]
        public IActionResult UpdateUserStatus([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { message = "User Data Missing" });

                var result = objRepo.UserRepository.UpdateUserStatus(user.Active? 1 : 0, user.Id);
                return Ok("User Status Updated Sucessfully.");

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }


        [HttpPost]
        [Route("UpdateUserPassword")]
        public IActionResult UpdateUserPassword([FromBody] User user)
        {
            try
            {
                if (user == null)
                    return BadRequest(new { message = "User Data Missing" });

                var result = objRepo.UserRepository.UpdateUserPassword(user);
                return Ok("User Password Updated Sucessfully.");

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }


        [HttpGet]
        [Route("GetUsersList")]
        public IActionResult GetUsersList()
        {
            try
            {
                // TODO : Check current user is QC/QA MANAGET or ADMIN
                IEnumerable<User> users = objRepo.UserRepository.GetUsersList();
                return Ok(users);

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }


        [HttpGet]
        [Route("GetUsersListByRoleByID/{RoleId}")]
        public IActionResult GetUsersListByRoleByID(int RoleId)
        {
            try
            {
                // TODO : Check current user is QC/QA MANAGET or ADMIN
                IEnumerable<User> users = objRepo.UserRepository.GetUsersListByRole(RoleId);
                return Ok(users);

            }
            catch( Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }

        [HttpGet]
        [Route("GetUsersListByRoles")]
        public IActionResult GetUsersListByRoles([FromQuery] int[] RoleId)
        {
            try
            {
                // TODO : Check current user is QC/QA MANAGET or ADMIN
                IEnumerable<User> users = objRepo.UserRepository.GetUsersListByRole(RoleId);
                return Ok(users);

            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }
    }
}

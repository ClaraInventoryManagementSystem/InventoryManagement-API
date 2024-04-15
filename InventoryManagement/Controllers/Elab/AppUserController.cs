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


namespace InventoryManagement.Controllers.Elab
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBaseEx
    {
        private readonly ILogger<AppUserController> _logger;
        private readonly IConfiguration _config;
        RepositoryCreator objRepo;
        public AppUserController(IConfiguration config, ILogger<AppUserController> logger)
        {
            _config = config;
            _logger = logger;
            //objBusinessCreator = new BusinessRepositoryCreator(_config);
            objRepo = new RepositoryCreator(_config);
        }

        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                var data = objRepo.AppUserRepository.GetRoles();
                if (data != null) return Ok(data);
                return NoContent();

            }
            catch (Exception eError)
            {
                return OnException(eError);
            }
        }

       

        //[HttpGet]
        //[Route("GetUsersListByRole/{RoleId}")]
        //public IActionResult GetUsersListByRole(int RoleId)
        //{
        //    try
        //    {
        //        // TODO : Check current user is QC/QA MANAGET or ADMIN
        //        IEnumerable<AppUser> users = objRepo.AppUserRepository.GetUsersListByRole(RoleId);
        //        return Ok(users);

        //    }
        //    catch( Exception eError)
        //    {
        //        _logger.LogError(eError.Message);
        //        return OnException(eError);
        //    }
        //}
        //[HttpGet]
        //[Route("GetUsersListByRoles")]
        //public IActionResult GetUsersListByRoles([FromQuery] int[] RoleId)
        //{
        //    try
        //    {
        //        // TODO : Check current user is QC/QA MANAGET or ADMIN
        //        IEnumerable<AppUser> users = objRepo.AppUserRepository.GetUsersListByRole(RoleId);
        //        return Ok(users);

        //    }
        //    catch (Exception eError)
        //    {
        //        _logger.LogError(eError.Message);
        //        return OnException(eError);
        //    }
        //}

       


    }
}

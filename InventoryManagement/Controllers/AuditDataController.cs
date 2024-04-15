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
using InventoryManagement.Common;

namespace InventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditDataController : ControllerBaseEx
    {

        private readonly ILogger<AuditDataController> _logger;
        private readonly IConfiguration _config;
        RepositoryCreator objRepo;
        public AuditDataController(IConfiguration config, ILogger<AuditDataController> logger)
        {
            _config = config;
            _logger = logger;
            //objBusinessCreator = new BusinessRepositoryCreator(_config);
            objRepo = new RepositoryCreator(_config);
        }

        [HttpGet]
        [Route("sampleauditdata")]
        public IActionResult GetAuditForSample( int inwardid)
        {
            IActionResult result = null;
            ResponseData response = null;
            try
            {
               
                response =  objRepo.AuditDataRepository.GetSampleAudit(inwardid, User.Identity.GetCurrentUserID(),
                    User.Identity.GetCurrentUserRoleId());

            }
            catch( Exception eError)
            {
                response = new ResponseData(DataResultType.Error,ErrorMessages.ERROR_UNKNOW_ERROR,500);
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
            finally
            {
                result =  PrepareAndSendResponse(response);
            }
            return result;
        }

        [HttpGet]
        [Route("GetSystemLog")]
        public IActionResult GetSystemLog(string FromDate, string ToDate,
                                         [FromHeader(Name = "ItemsPerPage")] string ItemsPerPage,
                                         [FromHeader(Name = "PageNo")] string PageNo,
                                         [FromHeader(Name = "SortBy")] string SortBy,
                                         [FromHeader(Name = "SortOrder")] string SortOrder)
        {
            IActionResult result = null;
            ResponseData response = null;
            int TotalRecords = 0;
            string ContentRange = "0";
            string PreviousPage = "0";
            string NextPage = "0";
            int MaxPage = 0;
            TableData t = null;
            Paging _paging = null;
            int perPage = this.ItemsPerPage;
            SortBy = this.SortBy;
            SortOrder = this.SortOrder;
            PageNo = this.PageNo.ToString();
            try
            {   

                TotalRecords = objRepo.AuditDataRepository.GetSystemLogTotalCount(FromDate, ToDate);
                Response.Headers.Add(WebHeaders.TotalRecords, TotalRecords.ToString());
               
                var page = PageResults(TotalRecords, out ContentRange, out PreviousPage, out NextPage, out MaxPage);
                if (page != null && page.Start > 0 && page.End > 0 && page.End >= page.Start)
                {
                    response =  objRepo.AuditDataRepository.GetSystemAudit(User.Identity.GetCurrentUserID(),User.Identity.GetCurrentUserRoleId(),
                        FromDate, ToDate, perPage, page.Start, page.End, SortBy, SortOrder);
                }
                else
                {
                    response = new ResponseData(DataResultType.BadRequest, ErrorMessages.ERROR_INVALID_REQUEST, 400);
                    
                }
                if( response != null)
                {
                    t = (TableData)response.Data;
                    _paging = new Paging();
                    _paging.ContentRange = ContentRange;
                    _paging.MaxPage = MaxPage;
                    _paging.NextPage = NextPage;
                    _paging.PreviousPage = PreviousPage;
                    _paging.TotalRecords = TotalRecords;
                    t.Paging = _paging;
                    response.Data = t;
                    
                }
                else
                {
                    if (response == null) response = new ResponseData();
                    response.DataType = DataResultType.Error;
                    response.Error = ErrorMessages.ERROR_UNEXPECTED_ERROR;
                }
                
            }
            catch ( Exception eError)
            {
                if (response == null) response = new ResponseData();
                response.DataType = DataResultType.Error;
                response.Error = ErrorMessages.ERROR_UNEXPECTED_ERROR;
                _logger.LogError(eError.Message);
                return OnException( eError);
            }
            finally
            {
                result = PrepareAndSendResponse(response);

            }
            return result;
        }



    }
}

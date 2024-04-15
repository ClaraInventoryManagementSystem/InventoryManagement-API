using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Models;
using InventoryManagement.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryManagement.cache;
using InventoryManagement.Common;
using System.Net.Http;
using System.Net;


namespace InventoryManagement.Controllers
{
    public class ControllerBaseEx : ControllerBase
    {


        protected string SortBy
        {
            get
            {
                string SortBy = String.Empty;
                if ((Request.Headers.ContainsKey(WebHeaders.SortBy) && !string.IsNullOrEmpty(Request.Headers[WebHeaders.SortBy].ToString())))
                    SortBy = Request.Headers[WebHeaders.SortBy].ToString();
                return SortBy;
            }
        }

        protected int ItemsPerPage
        {
            get
            {
                int ItemsPerPage = DBQueryConstant.DefaultPageSize;
                if ((Request.Headers.ContainsKey(WebHeaders.ItemsPerPage) && !string.IsNullOrEmpty(Request.Headers[WebHeaders.ItemsPerPage].ToString())))
                {
                    int.TryParse(Request.Headers[WebHeaders.ItemsPerPage].ToString(), out ItemsPerPage);
                }
                return ItemsPerPage;
            }

        }

        protected int PageNo
        {
            get
            {
                int pageNo = 1;
                if ((Request.Headers.ContainsKey(WebHeaders.PageNo) && !string.IsNullOrEmpty(Request.Headers[WebHeaders.PageNo].ToString())))
                {
                    int.TryParse(Request.Headers[WebHeaders.PageNo].ToString(), out pageNo);
                }
                return pageNo;
            }
        }

        protected string SortOrder
        {
            get
            {
                string sortOrder = String.Empty;
                if ((Request.Headers.ContainsKey(WebHeaders.SortOrder) && !string.IsNullOrEmpty(Request.Headers[WebHeaders.SortOrder].ToString())))
                {
                    sortOrder = Request.Headers[WebHeaders.SortOrder].ToString();
                }
                return sortOrder;
            }
        }




        protected IActionResult PrepareAndSendResponse(ResponseData response)
        {

            switch(response.DataType)
            {
                case DataResultType.Success:
                    return Ok(response.Data);
                case DataResultType.NoData:
                    return NoContent();
                case DataResultType.Error:
                    return StatusCode(500, response.Error);
            }
            return StatusCode(500,ErrorMessages.ERROR_UNEXPECTED_ERROR);
        }


        protected IActionResult OnException(Exception eError )
        {
            if( eError.GetType() == typeof(SecurityException) )
            {
                return Unauthorized();
            }
            ResponseData data = new ResponseData(DataResultType.Error, ErrorMessages.ERROR_UNKNOW_ERROR);
            return PrepareAndSendResponse(data);

        }
        

        public string RemoteIPAddress
        {
            get
            {
                if(HttpContext.Connection.RemoteIpAddress != null)
                {
                    return HttpContext.Connection.RemoteIpAddress.ToString();
                }
                return String.Empty;

            }
        }


        IRequestContext requestContext;
        IResponseContext responseContext;
        protected IRequestContext RequestContext
        {
            get
            {
                return requestContext;
            }
        }

        protected IResponseContext ResponseContext
        {
            get
            {
                return responseContext;
            }
        }

        public ControllerBaseEx()
        {

        }

        protected IActionResult RaiseError(string message, int ErrorCode)
        {
            ApiErrorResponse res = new ApiErrorResponse(message, ErrorCode);
            return NoContent();
        }


        protected Page PageResults(int total, out string ContentRange, out string PreviousPage, out string NextPage, out int MaxPage)
        {
            int start = 1, end = total;
            ContentRange = string.Empty;
            PreviousPage = string.Empty;
            NextPage = string.Empty;
            MaxPage = 0;
            Page pageInfo = null;
            //Request.Headers["TokenKey"].ToString(); 

            try
            {
                var pageSize = HttpContext.Request.Headers.ContainsKey(WebHeaders.ItemsPerPage) ? int.Parse(HttpContext.Request.Headers[WebHeaders.ItemsPerPage]) : 10;

                

                if (!HttpContext.Request.Headers.ContainsKey(WebHeaders.PageNo))
                {
                    //HttpContext.Response.SetProperties(Request.Properties, WebHeaders.AcceptRanges, WebHeaders.PageNo);
                    //ResponseContext.SetProperties((IDictionary<string, object>)Request.Headers, WebHeaders.AcceptRanges, WebHeaders.PageNo);
                    Response.Headers.Add(WebHeaders.AcceptRanges, WebHeaders.PageNo);
                }
                else
                {
                    int page = int.Parse(HttpContext.Request.Headers[WebHeaders.PageNo]);
                    var maxPage = total % pageSize == 0 ? total / pageSize : total / pageSize + 1;
                    if (page <= maxPage)
                    {
                        start = ((page - 1) * pageSize) + 1;
                        end = page * pageSize <= total ? page * pageSize : total;

                        ContentRange = string.Format("{0}-{1}/{2}", start, end, total);
                        PreviousPage = (start == 1 ? 0 : page - 1).ToString();
                        NextPage = (end < total ? page + 1 : 0).ToString();

                        Response.Headers.Add(WebHeaders.ContentRange, ContentRange);
                        Response.Headers.Add(WebHeaders.PreviousPage, PreviousPage);
                        Response.Headers.Add(WebHeaders.NextPage, NextPage);

                        //ResponseContext.SetProperties((IDictionary<string, object>)Request.Headers, WebHeaders.ContentRange, ContentRange);
                        //ResponseContext.SetProperties((IDictionary<string, object>)Request.Headers, WebHeaders.PreviousPage, PreviousPage);
                        //ResponseContext.SetProperties((IDictionary<string, object>)Request.Headers, WebHeaders.NextPage, NextPage);
                    }
                    else
                    {
                        start = 0;
                        end = 0;
                    }
                    Response.Headers.Add(WebHeaders.MaxPage, maxPage.ToString());
                    int.TryParse(maxPage.ToString(), out MaxPage);
                    // ResponseContext.SetProperties((IDictionary<string, object>)Request.Headers, WebHeaders.MaxPage, maxPage.ToString());
                }

            }
            catch( Exception eError)
            {
                

            }
            finally
            {

                pageInfo =  new Page() { Start = start, End = end };
                if( !(pageInfo.Start > 0 && pageInfo.End > 0 && pageInfo.End >= pageInfo.Start ))
                {
                    pageInfo.Start = 1;
                    pageInfo.End = 10;
                }
            }
            return pageInfo;

        }
        protected class Page
        {
            public override string ToString()
            {
                return string.Format("start={0} end={1}", Start, End);
            }
            public int Start;
            public int End;
        }

    }
}

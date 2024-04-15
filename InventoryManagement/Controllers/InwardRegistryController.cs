using System.Collections;
using System.Threading.Tasks;
using InventoryManagement.Business.Common;
using InventoryManagement.Models;
using InventoryManagement.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InventoryManagement.DataAccess.Common;
using System.Collections.Generic;
using MySqlX.XDevAPI.Relational;
using System;
using System.Linq;
using InventoryManagement.cache;
using Microsoft.AspNetCore.Cors;
using MySqlX.XDevAPI;
using InventoryManagement.Models.ViewModel;
using Org.BouncyCastle.Asn1.X509;
using System.Net.NetworkInformation;
using InventoryManagement.Models.Elab;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace InventoryManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InwardRegistryController : ControllerBaseEx
    {


        private readonly ILogger<InwardRegistryController> _logger;
        private readonly IConfiguration _config;
        private RepositoryCreator objRepo;

        public InwardRegistryController(IConfiguration config, ILogger<InwardRegistryController> logger)
        {
            _config = config;
            _logger = logger;
            objRepo = new RepositoryCreator(_config);
        }

        [HttpGet]
        [Route("GetList")]
        //[EnableCors("CorsPolicy")]
        public IActionResult GetList([FromHeader(Name = "filterCol")]  string filterCol, [FromHeader(Name = "filter")] string filter, 
                                         [FromHeader(Name = "ItemsPerPage")] string ItemsPerPage, [FromHeader(Name = "PageNo")] string PageNo,
                                       [FromHeader(Name = "SortBy")] string SortBy,[FromHeader(Name = "SortOrder")] string SortOrder)
        {            
            TableData data = null;
            int UserRole = -1;
            int TotalRecords = 0;
            int perPage = this.ItemsPerPage;
            int pageNo = this.PageNo;
            SortBy = this.SortBy;
            SortOrder = this.SortOrder;
            string FilterCondition = string.Empty;
            string FilterCondition1 = string.Empty;
            string strcolName = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(SortBy))
                {
                    SortBy = UIColumnCache.Instance.GetDbColumnName(SortBy);//AR_NUMBER
                    if (!string.IsNullOrEmpty(SortBy))
                    {
                        if (SortBy == "Customer")
                        {
                            SortBy = "cu.name";
                        }
                        else
                        {
                            SortBy = "iw." + SortBy;
                        }
                    }                                            
                }

                if (!string.IsNullOrEmpty(filterCol))
                {
                    strcolName = UIColumnCache.Instance.GetDbColumnName(filterCol);//AR_NUMBER

                    /* ArNumber ArType Customer Sample Category SubCategory IsRush SampleCondiation LotNumber ContainerCount ContainerType
                    Quantity Uom  Analyst CreatedBy CreateDate LastModifiedDate LastModifiedBy storagecondiation
                    resultsremarks status CustomerCode CustomerId InwardRegisterId */

                    if (!string.IsNullOrEmpty(strcolName))
                    {
                        if (strcolName == "Customer")
                        {
                            strcolName = "cu.name";
                        }
                        else
                        {
                            strcolName = "iw." + strcolName;
                        }
                    }
                    if (!string.IsNullOrEmpty(filter))
                        FilterCondition = strcolName + "= '" + filter + "'";
                }

                

                UserRole = User.Identity.GetCurrentUserRoleId();
                TotalRecords = objRepo.IRGridRepository.GetSampleListCount(UserRole, FilterCondition1);
                Response.Headers.Add(WebHeaders.TotalRecords, TotalRecords.ToString());

                string ContentRange = "0/0";
                string PreviousPage = "0"; 
                string NextPage ="0";
                int MaxPage = 0;
                Paging _paging = null;
                var page = PageResults(TotalRecords, out ContentRange, out PreviousPage, out NextPage, out MaxPage);
                if (page != null && page.Start > 0 && page.End > 0 && page.End >= page.Start)
                {
                    data = objRepo.IRGridRepository.GetGridData(UserRole, FilterCondition, perPage, page.Start, page.End, SortBy, SortOrder);                    
                }

                if (data != null)
                {
                    _paging = new Paging();
                    _paging.ContentRange = ContentRange;
                    _paging.MaxPage = MaxPage;
                    _paging.NextPage = NextPage;
                    _paging.PreviousPage = PreviousPage;
                    _paging.TotalRecords = TotalRecords;
                    data.Paging = _paging;
                }
            }
            catch( Exception eError)
            {
                _logger.LogError(eError.Message);
                 return OnException(eError); ;
            }
            finally
            {
                if( data != null && data.Columns != null && data.Columns.Count() == 0)
                {
                    data.Columns = UIColumnCache.Instance.GetScreenColumnsbyRole(1, UserRole);
                }
            }           
            return Ok(data);
        }

        [HttpGet]
        [Route("GetInwardRegDetailsByID")]
        public IActionResult GetInwardRegDetailsByID([FromHeader] int InwardRegisterId)
        {
            RegSampleInfo data = null;
            int UserRole = -1;
            try
            {
                if (InwardRegisterId == 0)
                    return BadRequest(new { message = "InwardRegisterId Data Missing" });

                UserRole = User.Identity.GetCurrentUserRoleId();
                data = objRepo.InwardRegistryRepository.GetInwardRegDetailsByID(InwardRegisterId);
                if (data != null && data.InwardRegisterId > 0)
                {
                    string fileDownloadURL = "";
                    if (_config != null && _config.GetSection("AppSettings:FileDownloadURL") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:FileDownloadURL").Value))
                        fileDownloadURL = _config.GetSection("AppSettings:FileDownloadURL").Value;

                    var testDetails =  objRepo.TestDetailsRepository.GetTestDetailsByInwardRegisterId(data.InwardRegisterId);                      
                    if (testDetails != null && testDetails.Any())
                    {                       
                        data.Tests = testDetails.ToList();
                    }
                    var checklistDetails = objRepo.ChecklistRepository.GetCheckListDetailsByInwardRegisterId(data.InwardRegisterId);
                    if (checklistDetails != null && checklistDetails.Any())
                    {
                        foreach (var item in checklistDetails)
                        {
                            item.FilePath = fileDownloadURL + item.FilePath;
                        }
                        data.FileDetails = checklistDetails.ToList();
                    }
                    return Ok(data);
                }
            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
            finally
            {
                // To do see how we can create dummy tables
            }
            return Ok(data);
        }


        [HttpPost]
        [Route("CreateInwardReg")]
        public IActionResult CreateInwardReg([FromBody] RegSampleInfo regSampleInfo)
        {
            try
            {
                if (regSampleInfo == null)
                    return BadRequest(new { message = "regSampleInfo Data Missing" });

                if (String.IsNullOrEmpty(regSampleInfo.ArNumber) && regSampleInfo.CustomerId > 0)
                {
                    regSampleInfo.ArNumber = regSampleInfo.CustomerCode;

                  var _cust =  objRepo.CustomerRepository.GetCustomerDetailsById(regSampleInfo.CustomerId);
                    if(_cust != null && _cust.Id > 0)
                    {
                        regSampleInfo.ArNumber = _cust.CustomerCODE;
                        regSampleInfo.CustomerCode = _cust.CustomerCODE;
                    }
                    string strNextSqr = string.Empty;
                    int number = 1;
                    string key_seq_number = "seqnumber";
                    var _settings = objRepo.SettingsRepository.GetSettingInfo();
                    if (_settings.Any())
                    {
                       var data = _settings.Where(x => x.Keyc == key_seq_number).FirstOrDefault();

                        if (data != null)
                        {
                            if (!String.IsNullOrEmpty(data.Valuec))
                            {
                                number = Convert.ToInt32(data.Valuec);
                            }
                        }
                        else
                        {
                            data = new Settings()
                            {
                                Id = 0,
                                Keyc = key_seq_number
                            };
                        }
                        String var = number.ToString("D4");
                        //String.Format("%04d", number); // var is "001"
                        number = number + 1;
                        data.Valuec = number.ToString();
                        objRepo.SettingsRepository.UpdateSettings(data);

                        string nextSeq = number.ToString();
                        //AR-RW-2024-003
                        strNextSqr = $" {"AR"}-{regSampleInfo.CustomerCode}-{DateTime.Now.ToString("yy")}{nextSeq}";
                    }

                    regSampleInfo.ArNumber = strNextSqr;
                }

                regSampleInfo.CreatedBy = User.Identity.GetCurrentUserID();
                //_logger.LogTrace("", regSampleInfo.Tests[0].);
                var result = objRepo.InwardRegistryRepository.CreateInwardReg(regSampleInfo);
                return Ok("Inward Info Updated Sucessfully.");
            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }

        [HttpPost]
        [Route("UpdateInwardReg")]
        public IActionResult UpdateInwardReg([FromBody] RegSampleInfo regSampleInfo)
        {
            string ErrorMeg = "";
            var result = false;
            int _role = 0;
            IrOperation _Operation = IrOperation.None;
            try
            {
                if (regSampleInfo == null)
                    return BadRequest(new { message = "regSampleInfo Data Missing" });

                //role

                if (Validate(regSampleInfo, regSampleInfo.bIsNew, out ErrorMeg))
                {
                    regSampleInfo.LastModifiedBy = User.Identity.GetCurrentUserID();

                    _role =  User.Identity.GetCurrentUserRoleId();
                    //User.Identity.GetCurrentUserRoleId();
                    _Operation = GetIrOperationState(regSampleInfo.Operation);
                    if (_Operation != IrOperation.None)
                    {
                        if (_Operation == IrOperation.Save)
                        {
                            result = SaveIREditInfo(regSampleInfo);
                        }
                        else if (_Operation == IrOperation.EnterResults)
                        {
                            result = SaveEnterResultsInfo(regSampleInfo);
                        }                                             
                        else if (_Operation == IrOperation.AssignAnalyst)
                        {
                            result = SaveAssignAnalystInfo(regSampleInfo);
                        }
                        else if (_Operation == IrOperation.ReAssignAnalyst)
                        {
                            result = SaveReAssignAnalystInfo(regSampleInfo);
                        }
                        return Ok("Inward Info Updated Sucessfully.");
                    }                    
                }
                return Ok("Inward Info Not Updated Sucessfully");
            }
            catch (Exception eError)
            {
                _logger.LogError(eError.Message);
                return OnException(eError);
            }
        }

        [HttpGet("GetChecklistsByMatterandProduct")]
        public IActionResult GetChecklistsByMatterandProduct(int materialId, int productId)
        {
            IEnumerable<FileDetail> checklists = null;
            if (materialId == 0 && productId == 0)
                return BadRequest(new { message = "Matter Id/Product Id Data Missing" });
            checklists = objRepo.ChecklistRepository.GetChecklistsByMatterandProduct(materialId, productId);
            if (checklists != null && checklists.Any())
            {
                string fileDownloadURL = "";
                if (_config != null && _config.GetSection("AppSettings:FileDownloadURL") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:FileDownloadURL").Value))
                    fileDownloadURL = _config.GetSection("AppSettings:FileDownloadURL").Value;
                foreach (var item in checklists)
                {
                    item.FilePath = fileDownloadURL + item.FilePath;
                }
            }
            return Ok(checklists);
        }
        [HttpGet("GetTestChecklistsByMatterProductAndTest")]
        public IActionResult GetTestChecklistsByMatterProductAndTest(int materialId, int productId,int testId)
        {
            IEnumerable<FileDetail> checklists = null;
            if (materialId == 0 && productId == 0 && testId ==0)
                return BadRequest(new { message = "Matter Id/Product Id/Test Id Data Missing" });
            checklists = objRepo.ChecklistRepository.GetTestChecklistsByMatterProductAndTest(materialId, productId,testId);
            if (checklists != null && checklists.Any())
            {
                string fileDownloadURL = "";
                if (_config != null && _config.GetSection("AppSettings:FileDownloadURL") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:FileDownloadURL").Value))
                    fileDownloadURL = _config.GetSection("AppSettings:FileDownloadURL").Value;
                foreach (var item in checklists)
                {
                    item.FilePath = fileDownloadURL + item.FilePath;
                }
            }
            return Ok(checklists);
        }

        [HttpGet("GetChecklistsByInwRegIdandTestId")]
        public IActionResult GetChecklistsByInwRegIdandTestId(int inwardRegisterId, int testId)
        {
            IEnumerable<FileDetail> checklists = null;
            if (inwardRegisterId == 0 && testId == 0)
                return BadRequest(new { message = "Inward Register Id/Test Id Data Missing" });
            checklists = objRepo.ChecklistRepository.GetChecklistsByInwRegIdandTestId(inwardRegisterId, testId);
            if (checklists != null && checklists.Any())
            {
                string fileDownloadURL = "";
                if (_config != null && _config.GetSection("AppSettings:FileDownloadURL") != null && !string.IsNullOrWhiteSpace(_config.GetSection("AppSettings:FileDownloadURL").Value))
                    fileDownloadURL = _config.GetSection("AppSettings:FileDownloadURL").Value;
                foreach (var item in checklists)
                {
                    item.FilePath = fileDownloadURL + item.FilePath;
                }
            }
            return Ok(checklists);
        }

        private bool SaveEnterResultsInfo(RegSampleInfo regSampleInfo)
        {
            bool bResult = false;
            bool bChangedStatus = true;
            foreach (var test in regSampleInfo.Tests)
            {
                if (String.IsNullOrEmpty(test.result))
                    bChangedStatus = false;
            }
            if (bChangedStatus)
            {
                regSampleInfo.Status = "Review";  //"Closed";
            }
            
            objRepo.InwardRegistryRepository.UpdateInwardReg(regSampleInfo);
            return bResult;
        }
        private bool SaveAssignAnalystInfo(RegSampleInfo regSampleInfo)
        {
            bool bResult = false;
            bool bChangedStatus = true;
            foreach (var test in regSampleInfo.Tests)
            {
                if (String.IsNullOrEmpty(test.result))
                    bChangedStatus = false;
            }
            if (bChangedStatus)
            {
                regSampleInfo.Status = "Closed";
            }
            else
            {
                regSampleInfo.Status = "Analysis initiated";
            }

            regSampleInfo.AssignedBy = User.Identity.GetCurrentUserID();
            if (string.IsNullOrEmpty(regSampleInfo.Analyst) || regSampleInfo.Analyst.Trim() != "")
                regSampleInfo.Analyst = regSampleInfo.Tests[0].analyst.ToString();

            objRepo.InwardRegistryRepository.SaveAssignAnalystInfo(regSampleInfo);
            return bResult;
        }

        private bool SaveReAssignAnalystInfo(RegSampleInfo regSampleInfo)
        {
            bool bResult = false;
            bool bChangedStatus = true;

            //foreach (var test in regSampleInfo.Tests)
            //{
            //    if (String.IsNullOrEmpty(test.result))
            //        bChangedStatus = false;
            //}
            //if (bChangedStatus)
            //{
            //    regSampleInfo.Status = "Closed";
            //}
            //else
            //{
            //    regSampleInfo.Status = "Analysis initiated";
            //}


            regSampleInfo.Status = "Analysis initiated";

            regSampleInfo.AssignedBy = User.Identity.GetCurrentUserID();
            if (string.IsNullOrEmpty(regSampleInfo.Analyst.Trim()))
                regSampleInfo.Analyst = regSampleInfo.Tests[0].analyst.ToString();

            objRepo.InwardRegistryRepository.SaveAssignAnalystInfo(regSampleInfo);
            return bResult;
        }
        private bool SaveIREditInfo(RegSampleInfo regSampleInfo)
        {
            bool bResult = false;
            bool bChangedStatus = true;
            foreach (var test in regSampleInfo.Tests)
            {
                if (String.IsNullOrEmpty(test.result))
                    bChangedStatus = false;
            }
            if (bChangedStatus && !string.IsNullOrEmpty(regSampleInfo.Status) && regSampleInfo.Status.ToLower() == "review")
            {
                regSampleInfo.Status = "Closed";
            }
            objRepo.InwardRegistryRepository.UpdateInwardReg(regSampleInfo);
            return bResult;
        }
        
        private IrOperation GetIrOperationState(int _operation)
        {
            IrOperation _Operation = IrOperation.None;
            if (_operation == IrOperation.AssignAnalyst.GetHashCode())
                _Operation = IrOperation.AssignAnalyst;
            else if(_operation == IrOperation.EnterResults.GetHashCode())
                  _Operation = IrOperation.EnterResults;
            else if (_operation == IrOperation.Save.GetHashCode())
                _Operation = IrOperation.Save;
            else if (_operation == IrOperation.ReAssignAnalyst.GetHashCode())
                _Operation = IrOperation.ReAssignAnalyst;

            return _Operation;
        }
        private bool Validate(RegSampleInfo regSampleInfo, bool bIsNew, out string ErrorMeg)
        {
            ErrorMeg = "";
            if (regSampleInfo != null)
            {
                if (String.IsNullOrEmpty(regSampleInfo.CustomerName)) 
                {
                    ErrorMeg = "Invalid Customer.";                    
                    return false;
                }

                if (String.IsNullOrEmpty(regSampleInfo.Sample))
                {                    
                    ErrorMeg = "Invalid Sample.";
                    return false;
                }

                //if (String.IsNullOrEmpty(regSampleInfo.Category))
                //{
                //    ErrorMeg = "Invalid Category.";
                //    return false;
                //}

                //if (String.IsNullOrEmpty(regSampleInfo.ArType))
                //{
                //    ErrorMeg = "Invalid ArType.";
                //    return false;
                //}

                //if (String.IsNullOrEmpty(regSampleInfo.SubCategory))
                //{
                //    ErrorMeg = "Invalid Sub Category.";
                //    return false;
                //}

                //if (String.IsNullOrEmpty(regSampleInfo.IsRush))
                //{
                //    ErrorMeg = "Invalid IsRush.";
                //    return false;
                //}

                if (String.IsNullOrEmpty(regSampleInfo.LotNumber))
                {
                    ErrorMeg = "Invalid Lot Number.";
                    return false;
                }

                if (String.IsNullOrEmpty(regSampleInfo.Quantity.ToString()))
                {
                    ErrorMeg = "Invalid Quantity.";
                    return false;
                }

                //if (String.IsNullOrEmpty(regSampleInfo.StorageCondiation))
                //{
                //    ErrorMeg = "Invalid Storage Method.";
                //    return false;
                //}

                if (regSampleInfo.InwardRegisterId == 0 && !regSampleInfo.Tests.Any())
                {
                    ErrorMeg = "Invalid Tests.";
                    return false;
                }
            }
            return true;
        }

        private Customer validateAndSaveCustomer(RegSampleInfo regSampleInfo)
        {
            List<Customer> customers = null;
            Customer c = null;
            //bool isNew = false;
            if (c == null)
            {                
                customers = objRepo.CustomerRepository.GetCustomerDetailsByName(regSampleInfo.CustomerName, regSampleInfo.CustomerCode).ToList();

                if (customers != null && customers.Any())
                {
                    c = customers.FirstOrDefault();
                    if (c != null && c.Id> 0)
                    {
                        return c;
                    }                    
                }
                c = new Customer()
                {
                    Id = 0,
                    Name = regSampleInfo.CustomerName,
                    Code = regSampleInfo.CustomerCode
                };

              var CustomerId =  objRepo.CustomerRepository.AddCutomer(c);
            }
            return c;
        }


    }
}

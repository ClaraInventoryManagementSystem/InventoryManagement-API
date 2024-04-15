using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Common
{

    internal static class WebHeaders
    {
        internal static string TokenKey = "TokenKey";
        internal static string ApiKey = "ApiKey"; 
        internal static string ActiveToken = "ActiveToken"; 
        internal static string TokenExpires = "TokenExpires";
        internal static string LoginId = "LoginId";
        internal static string Email = "Email";
        internal static string Password = "Password";
        internal static string NewPassword = "NewPassword";
        internal static string NewLoginId = "NewLoginId";

        internal static string ResponseStatus = "ResponseStatus";
        internal static string PageNo = "PageNo";//"Page";
        internal static string ItemsPerPage = "ItemsPerPage";// "PerPage";
        internal static string ContentRange = "Content-Range";
        internal static string AcceptRanges = "Accept-Ranges";
        internal static string PreviousPage = "Previous-Page";
        internal static string NextPage = "Next-Page";
        internal static string MaxPage = "Max-Page";
        internal static string TotalRecords = "TotalRecords";
        
        internal static string Prefix = "Header.";
        internal static string Permission = "Permission";

        internal static string SortBy = "SortBy";
        internal static string SortOrder = "SortOrder";
        internal static string TimeZoneID = "TimeZoneID";
        internal static string TimeDiff = "TimeDiff";
    }

    internal static class QueryConstant
    {
        internal static string ProjectAccepted = "accepted";
        internal static string ProjectNotYetResponded = "notyetresponded";
        internal static string ProjectDenied = "denied";
    }


    internal static class ApplicationConstant
    {
        internal static string Domain = "Domain";
        internal static string Virtual = "Virtual";        
        internal static string IsSingleSignOn = "IsSingleSignOn";
        internal static string SessionTimeOutCheck = "SessionTimeOutCheck";
        internal static string SessionExpiry = "SessionExpiry";
        internal static string ServerTimeZone = "ServerTimeZone";
    }


    public enum ErrorList
    {
        UnknownException = 0,
        InvalidToken = 1,
        EmptyToken = 2,
        InvalidCredential = 3,        
        UnableToProcess = 4,        
        AlreadyExists = 5,
        PermissionDenied = 6,
        DuplicateEmail = 7,
        SeatUnavailable = 8,
        NoImageFound = 9,
        DuplicateContact = 10,
        UnableToLogin = 11,
        LoginIdUnavailable = 12,
        AccountInactive = 13,
        EmptySubject = 14,
        EmptyMessage = 15,
        EmptyEmail = 16,
        InvalidEmailFormat = 17,
        FolderAlreadyExists = 18,        
        EmptySessionID = 19,
        AuthorizationFailed = 20,
        DuplicateUserID = 21,
        MissingData = 22
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;
using InventoryManagement.Common;

namespace InventoryManagement.DataAccess.Common.Interface
{
    public interface IAuditDataRepository
    {

        bool AddNewSystemAuditLog(SystemAuditRecord r);

        ResponseData GetSampleAudit(int  inward_id, int user, int nRole);

        int GetSystemLogTotalCount(string startDate, string endData);

        ResponseData GetSystemAudit(int userid, int userRole,string startdate, string enddate, int perPage, int pageStart, int pageEnd, string SortBy, string SortOrder);
    }
}

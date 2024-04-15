using InventoryManagement.Common;
using InventoryManagement.Models;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryManagement.DataAccess.Common.Interface
{
    public interface IIRGridRepository
    {
        public int GetSampleListCount(int nRole, string Filter = null);
        public TableData GetGridData(int nRole, string Filter ,int perPage, int pageStart, int pageEnd, string SortBy, string SortOrder);

      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;

namespace InventoryManagement.DataAccess.Common.Interface
{
    public interface IAppRoleUIColumnRepository
    {

        public IEnumerable<TableColumnData> GetAllColumns();
    }
}

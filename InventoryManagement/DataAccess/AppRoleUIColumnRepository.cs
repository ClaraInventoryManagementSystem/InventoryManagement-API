using Dapper;
using InventoryManagement.Common;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.DataAccess.Common.Interface;
using InventoryManagement.DataAccess.Elab.Interface;
using InventoryManagement.Models;
using InventoryManagement.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess
{
    public class AppRoleUIColumnRepository : DataAccessRepository<TableColumnData>,IAppRoleUIColumnRepository
    {

        public AppRoleUIColumnRepository(IConfiguration config) : base(config)
        {

        }

        public IEnumerable<TableColumnData> GetAllColumns()
        {
            DynamicParameters p = new DynamicParameters();
            IDbConnection conn = GetConnection();
           return GetRecord<TableColumnData>(DBQueryConstant.Get_App_Role_UI_Columns_All, p, CommandType.Text);
        }
    }
}

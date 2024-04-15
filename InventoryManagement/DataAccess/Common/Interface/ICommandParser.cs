using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess;


namespace InventoryManagement.DataAccess.Common
{
    public interface ICommandParser
    {
        DbCommand GetCommand<TItem>(DbConnection connection, Method method, params object[] values);
        DbCommand GetCommand(string key, DbConnection connection, Method method, params object[] values);
        DbCommand GetCommand(string query, DbConnection connection);
    }
    public enum Method
    {
        CREATE,
        READ,
        UPDATE,
        DELETE,
        COUNT
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.Models;
using InventoryManagement.DataAccess.Common.Interface;

using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using InventoryManagement.Common;
using Serilog;
using MySql;
using MySql.Data.MySqlClient;
using InventoryManagement.cache;

using LovGroup = InventoryManagement.Common.LovGroup;
using System.IO.Pipelines;
using MySqlX.XDevAPI;
namespace InventoryManagement.DataAccess.Common
{
    public class SettingsRepository : DataAccessRepository<object>, ISettingsRepository
    {

        public SettingsRepository(IConfiguration config) : base(config)
        {

        }
        public IEnumerable<KeyValuePair<string,string>> GetSettings()
        {
            DynamicParameters param = new DynamicParameters();
            return  GetRecord<KeyValuePair<string,string>>(DBQueryConstant.GET_SETTINGS_ALL, param, CommandType.Text);
            
        }

        public IEnumerable<Settings> GetSettingInfo()
        {
            DynamicParameters param = new DynamicParameters();
            return GetRecord<Settings>(DBQueryConstant.GET_SETTINGS_ALL, param, CommandType.Text);
        }

        public bool UpdateSettings(Settings settings)
        {            
            bool bresult = false;
            int result = 0;
            var p = new DynamicParameters();
            p.Add("@value", settings.Valuec);
            p.Add("@key", settings.Keyc);
            result = UpdateRecord(DBQueryConstant.UPDATE_SETTINGS, ref p, CommandType.Text);
            if (result == 1)
                bresult = true;
            return bresult;
        }
    }
}

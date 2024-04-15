using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Models;

namespace InventoryManagement.DataAccess.Common.Interface
{
    public interface ISettingsRepository
    {

        IEnumerable<KeyValuePair<string,string>> GetSettings();

        IEnumerable<Settings> GetSettingInfo();

        bool UpdateSettings(Settings settings);
    }
}

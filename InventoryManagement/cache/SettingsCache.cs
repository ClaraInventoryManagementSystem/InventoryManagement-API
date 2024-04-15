using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using InventoryManagement.DataAccess;
using InventoryManagement.DataAccess.Common;


namespace InventoryManagement.cache
{
    public class SettingsCache : CacheBase
    {

        public static string KEY_RSN_CC_EMAIL_LIST = "RSN_CC_EMAIL_LIST";
        public static string KEY_DOC_STORAGE_PATH = "DOC_STORAGE_PATH";
        public static string KEY_SAVE_COA_PDF = "SAVE_COA_PDF_TO_DISK";

        public static string ENABLE_REGISTER_EMAIL = "ENABLE_REGISTER_EMAIL";
        public static string REGISTER_FROM_EMAIL = "REGISTER_FROM_EMAIL";
        public static string REGISTER_FROM_EMAIL_PWD = "REGISTER_FROM_EMAIL_PWD";
        public static string REGISTER_CC_EMAIL = "REGISTER_CC_EMAIL";

        public static string ENABLE_COA_EMAIL = "ENABLE_COA_EMAIL";
        public static string COA_FROM_EMAIL = "COA_FROM_EMAIL";
        public static string COA_FROM_EMAIL_PWD = "COA_FROM_EMAIL_PWD";
        public static string COA_CC_EMAIL = "COA_CC_EMAIL";





        private static SettingsCache _Instance = null;

        public static void InitCache(IConfiguration config)
        {
            _Instance = new SettingsCache(config);
        }

        public static SettingsCache Instance
        {
            get
            {
                return _Instance;
            }
        }


        private RepositoryCreator obeReproCreator;

        private Dictionary<string, string> _settingsCache = new Dictionary<string, string>();


        private SettingsCache(IConfiguration config) : base(config)
        {
            obeReproCreator = new RepositoryCreator(_config);
            Build(true);

        }
        public override bool Build(bool bForece = false)
        {
            try
            {
                IEnumerable<KeyValuePair<string,string>> list = obeReproCreator.SettingsRepository.GetSettings();

                _lock.EnterWriteLock();
                _settingsCache.Clear();
                foreach( KeyValuePair<string,string> pair in list)
                {
                    _settingsCache.Add(pair.Key, pair.Value);
                }
                Log.Logger.Information(">>> settings cache build size ={0}", _settingsCache.Count);

            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message);
                return false;
            }
            finally
            {
                if (_lock.IsWriteLockHeld)
                    _lock.ExitWriteLock();
            }
            return true;
        }

        public string GetValue(string key)
        {
            string strValue = String.Empty;
            try
            {
                _lock.EnterReadLock();
                if( _settingsCache != null)
                {
                    if (_settingsCache.ContainsKey(key))
                        strValue = _settingsCache[key];
                }
            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message);

            }
            finally
            {
                if (_lock.IsReadLockHeld)
                    _lock.ExitReadLock();

            }
            return strValue;
        }
    }

   

}

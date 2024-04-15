using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Hosting;

namespace InventoryManagement.cache
{
    public class ApplicationCache
    {
        private static IConfiguration _config;

        public static IConfiguration Config
        {
            get
            {
                return _config;
            }
        }

        private static IHostEnvironment _hostEnv;

        public static IHostEnvironment HostEnv
        {
            get
            {
                return _hostEnv;
            }
         
        }
          

        private ApplicationCache(IConfiguration config, IHostEnvironment env)
        {
            _config = config;
            _hostEnv = env;
            _Initcache();

        }

        private static void _Initcache()
        {
            Log.Information(">>>>>>>>> building cache start  >>>>");
            UIColumnCache.InitCache(_config);
            Log.Information(">>>>>>>>> building cache end  >>>>");

            SettingsCache.InitCache(_config);
            Log.Information(">>>>building setting cache end >>>>");

            Log.Information(">>>>building Material Cache setting cache end >>>>");

        }

        public static void Init(IConfiguration config,IHostEnvironment env)
        {
            _instance = new ApplicationCache(config,env);
        }

        private static ApplicationCache _instance;

        public static ApplicationCache Instance
        {
            get
            {
                return _instance;
            }
        }

        
    }
}

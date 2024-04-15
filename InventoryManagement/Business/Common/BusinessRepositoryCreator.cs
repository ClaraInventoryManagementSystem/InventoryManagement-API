using InventoryManagement.Business.Common.Interface;
//using InventoryManagement.Business.Assets.Interface;
//using InventoryManagement.Business.HR.Interface;
//using InventoryManagement.Business.HR;
//using InventoryManagement.Business.Assets;
using InventoryManagement.Business.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryManagement.Business.Elab;
using InventoryManagement.Business.Elab.Interface;

namespace InventoryManagement.Business.Common
{
    public class BusinessRepositoryCreator
    {
        private static IConfiguration _Config { get; set; }
        public BusinessRepositoryCreator(IConfiguration Config)
        {
            _Config = Config;
        }

        
        
        
        public ITokenServices TokenServices
        {
            get
            {
                return new TokenServices(_Config);
            }
        }

        public IAppUserBusiness AppUserBusiness
        {
            get
            {
                return new AppUserBusiness(_Config);
            }
        }
       

    }
}

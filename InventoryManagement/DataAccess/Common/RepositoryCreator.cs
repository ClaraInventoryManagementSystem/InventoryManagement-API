//using InventoryManagement.DataAccess.Assets;
//using InventoryManagement.DataAccess.Assets.Interface;
using InventoryManagement.DataAccess.Common;
using InventoryManagement.DataAccess.Common.Interface;
using InventoryManagement.DataAccess.Elab;
using InventoryManagement.DataAccess.Elab.Interface;
//using InventoryManagement.DataAccess.HR;
//using InventoryManagement.DataAccess.HR.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace InventoryManagement.DataAccess.Common
{
    public class RepositoryCreator
    {
        private static IConfiguration _Config { get; set; }

        private readonly IHostEnvironment _hostEnvironment;


        

        public RepositoryCreator(IConfiguration Config)
        {
            _Config = Config;
        }

        public RepositoryCreator(IConfiguration Config, IHostEnvironment hostEnvironment)
        {
            _Config = Config;
            _hostEnvironment = hostEnvironment;
        }


        public IAuditDataRepository AuditDataRepository
        {
            get
            {
                return new AuditDataRepository(_Config);
            }
        }


        public ISettingsRepository SettingsRepository
        {
            get
            {
                return new SettingsRepository(_Config);
            }

        }


       

        

       
       

        public IAppRoleUIColumnRepository RoleUIColumnRepository
        {
            get
            {
                return new AppRoleUIColumnRepository(_Config);
            }
        }
            


        public ITokenRepository TokenRepository
        {
            get
            {
                return new TokenRepository(_Config);
            }
        }

        public IAppUserRepository AppUserRepository
        {
            get
            {
                return new AppUserRepository(_Config);
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                return new UserRepository(_Config);
            }
        }
    }
}
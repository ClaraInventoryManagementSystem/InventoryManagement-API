using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.DataAccess.Common
{
    public abstract class BaseDataAccessRepositorycs : IDisposable
    {
        private IConfiguration _Config;
        //protected string connStringName = "Server=192.168.8.200;Uid=FLEXI;pwd=admin;Port=3306;Database=FLEXI;ConnectionTimeout=120;MaximumPoolsize=200;";
        //public IDbConnection Connection;

        /*protected BaseDataAccessRepositorycs(string connectionStringName)
        {
            //if (!string.IsNullOrWhiteSpace(connectionStringName))
            //    this.Connection = new MySqlConnection(connectionStringName);
            //else
            //    Connection = new MySqlConnection(connStringName);
        }*/

        protected BaseDataAccessRepositorycs(IConfiguration Config)
        {
            _Config = Config;
            //if (_Config != null && _Config.GetSection("AppSettings:ConnStringName") != null && !string.IsNullOrWhiteSpace(_Config.GetSection("AppSettings:ConnStringName").Value))
            //    this.Connection = new MySqlConnection(_Config.GetSection("AppSettings:ConnStringName").Value);
            //else
            //    Connection = new MySqlConnection(connStringName);
        }

        public abstract IDbConnection GetConnection();

        public abstract void CreateConnection();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}

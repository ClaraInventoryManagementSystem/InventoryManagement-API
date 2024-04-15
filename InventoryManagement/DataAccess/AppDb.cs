using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FLEXIAPI.DataAccess
{
    public class AppDb : IDisposable
    {
        public MySqlConnection Connection;

        public AppDb()
        {
            Connection = new MySqlConnection("server=127.0.0.1;user id=root;password=root;port=3306;database=sakila;");
        }

        public void Dispose()
        {
            Connection.Close();
        }
    }
}

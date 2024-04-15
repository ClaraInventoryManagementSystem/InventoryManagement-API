using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using MySql.Data.MySqlClient;
using InventoryManagement.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
/// <summary>
/// Pending Items in this Class:
/// 1. Get multiple results
/// 2. Batch execution
/// </summary>
/// 
namespace InventoryManagement.DataAccess.Common
{
    public abstract class DataAccessRepository<TItem> : BaseDataAccessRepositorycs where TItem : class, new()
    {
        protected IConfiguration _Config;
        public IDbConnection Connection;
        internal DataAccessRepository(IConfiguration Config)
            : base(Config)
        {
            _Config = Config;
            CreateConnection();
        }

        /*internal SqlDataAccessRepository(string connectionStringName)
            : base(connectionStringName)
        {
            CreateConnection();
        }*/

        public override void CreateConnection()
        {
            string connString = "";
            if (_Config != null && _Config.GetSection("AppSettings:ConnStringName") != null && !string.IsNullOrWhiteSpace(_Config.GetSection("AppSettings:ConnStringName").Value))
                connString = _Config.GetSection("AppSettings:ConnStringName").Value;
            try
            {
                if (connString != null && connString.Length != 0)
                    this.Connection = new MySqlConnection(_Config.GetSection("AppSettings:ConnStringName").Value);
                else
                    throw new Exception("Missing Database Connection String");
            }
                
            catch (Exception e)
            {
                throw e;
                
            }
        }

        public override IDbConnection GetConnection()
        {
            return Connection;
        }

        //protected bool recordExist = false;
        //protected int readerIndex = 0; /*used for identifying how many record set need to skip from the reader object*/
        //protected int outParams = 0; /*used for identifying how many out parameter used in sp*/
        //protected int totRecordCount = 0; /*used for identifying how many records having the reader object*/
        //protected Dictionary<string, string> outContext; /*used for containing out parameter values*/

        #region Private Functions
        /*
        private IEnumerable<PropertyInfo> GetObjectProperties(Type type)
        {
            // TODO - Consider property attributes and types
            return type.GetProperties();
        }

        private IDictionary<string, PropertyInfo> GetObjectPropertyInfo(Type type)
        {
            var retval = new Dictionary<string, PropertyInfo>();
            foreach (var property in type.GetProperties())
            {
                retval.Add(property.Name, property);
            }
            return retval;
        }
        */
        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// To get the output paramenter value --> p.Get<int>("<Parametername>");
        /// </summary>
        /// <param name="CommandText"></param>
        /// <param name="CommandParameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        protected int CreateRecord(string CommandText, ref DynamicParameters CommandParameters, CommandType commandType)
        {
            int success = -1;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    connection.Execute(CommandText, CommandParameters, commandType: commandType);
                    success = 1;
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //connection.Close();
            }
            return success;
        }

        protected int CreateRecordWithinTransaction(string CommandText, ref DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction)
        {
            int success = -1;            
            if (transaction != null && transaction.Connection != null)
            {                
                transaction.Connection.Execute(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                success = 1;
            }
            return success;
        }

        protected int CreateRecordNoResult(string CommandText, DynamicParameters CommandParameters, CommandType commandType)
        {
            int success = -1;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    connection.Execute(CommandText, CommandParameters, commandType: commandType);
                    success = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        protected int CreateRecordNoResultWithinTransaction(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction)
        {
            int success = -1;

            if (transaction != null && transaction.Connection != null)
            {
                transaction.Connection.Execute(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                success = 1;
            }

            return success;
        }

        protected int DeleteRecord(string CommandText, DynamicParameters CommandParameters, CommandType commandType)
        {
            int success = -1;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    connection.Execute(CommandText, CommandParameters, commandType: commandType);
                    success = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        protected int DeleteRecordWithinTransaction(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction)
        {
            int success = -1;
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Connection.Execute(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                success = 1;
            }

            return success;
        }

        protected int UpdateRecord(string CommandText, ref DynamicParameters CommandParameters, CommandType commandType)
        {
            int success = -1;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    connection.Execute(CommandText, CommandParameters, commandType: commandType);
                    success = 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return success;
        }

        protected int UpdateRecordWithinTransaction(string CommandText, ref DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction)
        {
            int success = -1;
            if (transaction != null && transaction.Connection != null)
            {
                transaction.Connection.Execute(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                success = 1;
            }

            return success;
        }

        #endregion

        #region ExecuteReader

        protected T GetRecordById<T>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null) where T : new()
        {
            T result = new T();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    var resultList = connection.Query<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                    if (resultList != null && resultList.Count() > 0)
                        result = resultList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {                
            }
            return result;
        }

        //QueryAsync
        protected async Task<T> GetRecordByIdAsync<T>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null) where T : new()
        {
            T result = new T();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    var resultList = await connection.QueryAsync<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                    if (resultList != null && resultList.Count() > 0)
                        result = resultList.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return result;
        }
        protected IEnumerable<T> GetRecord<T>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null) where T : new()
        {
            IEnumerable<T> result = null;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    result = connection.Query<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        protected async Task<IEnumerable<T>> GetRecordAsync<T>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null) where T : new()
        {
            IEnumerable<T> result = null;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    result = await connection.QueryAsync<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        // need to check what it is used for
        /* protected int GetOrdinalRecordData(string caption, params object[] values)
         {
             using (DbConnection connection = GetConnection())
             {
                 connection.Open();

                 using (var dbCommand = CommandParser.Current.PrepareCommand<TItem>(connection, Method.ORDINAL, caption, values))
                 {
                     using (var reader = dbCommand.ExecuteReader())
                     {
                         var rowNumber = -1;
                         while (reader.Read())
                             int.TryParse(reader["RowNumber"].ToString(), out rowNumber);

                         reader.Close();
                         if (connection.State == ConnectionState.Open)
                         {
                             connection.Close();
                             connection.Dispose();
                         }
                         return rowNumber;

                     }
                 }
             }
         }

         protected int GetOrdinalRecordEx(string caption, params object[] values)
         {
             using (DbConnection connection = GetConnection())
             {
                 connection.Open();

                 using (var dbCommand = CommandParser.Current.PrepareCommand<TItem>(connection, Method.ORDINAL, caption, values))
                 {
                     using (var reader = dbCommand.ExecuteReader())
                     {
                         var rowNumber = -1;
                         while (reader.Read())
                             int.TryParse(reader["IsPermitted"].ToString(), out rowNumber);

                         reader.Close();
                         if (connection.State == ConnectionState.Open)
                         {
                             connection.Close();
                             connection.Dispose();
                         }
                         return rowNumber;

                     }
                 }
             }
         }
         */

        #endregion

        #region ExecuteScalar

        protected int GetRecordCount(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null)
        {
            var count = 0;
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    var result = connection.Query<int>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                    if (result != null && result.Count() > 0)
                        count = result.Single();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return count;
        }

        protected int GetSingleIntValueRecordWithinTransaction(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction)
        {
            int dataValue = 0;
            if (transaction != null && transaction.Connection != null)
            {
                var result = transaction.Connection.Query<int>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                if (result != null && result.Count() > 0)
                    dataValue = result.Single();
            }
            return dataValue;
        }
        
        //Need to test the below implementation.
        //If not working, will split into multiple functions with return type -> int / string.
        protected T GetScalarRecord<T>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null) where T : new()
        {
            T scalarValue = new T();
            try
            {
                using (IDbConnection connection = GetConnection())
                {
                    connection.Open();
                    var result = connection.Query<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
                    if (result != null && result.Count() > 0)
                        scalarValue = result.Single();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return scalarValue;
        }

        #endregion

        #region ExexuteBatch

        int defaultBatchCount = 5;
        // Need to implement later
        //protected string BatchCommand(string caption, Method method, params object[] values)
        //{
        //    var ns = typeof(TItem).ToString().Split('.');
        //    var key = ns[ns.Length - 1];

        //    return CommandParser.Current.PrepareCommand(key, method, caption, values);
        //}
        
         //To execute string or varchar parameter value every time one single quote need to be added to execute the command eg: val=abc need to be ''abc''
        protected bool BatchExecution(DbTransaction transaction, StringCollection commands, int batchCount)
        {
            bool success = false;

            if (batchCount == 0)
                batchCount = defaultBatchCount;

            int commandCount = commands.Count;
            int totalBatches = commandCount / batchCount;

            if ((commandCount % batchCount) == 0)
                totalBatches = commandCount / batchCount;
            else
                totalBatches = (commandCount / batchCount) + 1;

            int currentBatch = default(int);
            int currentItemCount = default(int);
            string executeCommand = "sp_executesql  N' ";
            string[] finalCommands = new string[totalBatches];
            bool finalLeft = false;

            for (int loop = 0; loop < commandCount; loop++)
            {
                if ((commands[loop] != null) && (commands[loop] != string.Empty))
                {
                    if (currentItemCount == batchCount)
                    {
                        if (commands[loop] != string.Empty)
                        {
                            executeCommand += commands[loop];
                            executeCommand += ";";
                            executeCommand += "'";
                            currentItemCount = default(int);
                            finalCommands[currentBatch++] = executeCommand;
                            executeCommand = "sp_executesql  N' ";
                            finalLeft = false;
                        }
                    }
                    else
                    {
                        if (commands[loop] != string.Empty)
                        {
                            executeCommand += commands[loop];
                            executeCommand += ";";
                            currentItemCount++;
                            finalLeft = true;
                        }
                    }
                }
            }

            if (finalLeft)
            {
                executeCommand += "'";
                currentItemCount = default(int);
                finalCommands[currentBatch++] = executeCommand;
                finalLeft = false;
            }

            //Now Loop through the each item and execute
            commandCount = finalCommands.Length;
            if (commandCount > 0)
            {
              // var objTrans = objConn.BeginTransaction();
               bool bTransStarted = true;

                for (int loop = 0; loop < commandCount; loop++)
                {
                    executeCommand = finalCommands[loop];
                    if ((executeCommand != null) && (executeCommand != string.Empty))
                    {
                        if (transaction != null)
                        {
                            using (var command = transaction.Connection.CreateCommand())
                            {
                                command.Transaction = transaction;
                                command.CommandType = CommandType.Text;
                                command.CommandText = executeCommand;
                                command.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            using (IDbConnection connection = GetConnection())
                            {
                                connection.Open();
                                using (var command = connection.CreateCommand())
                                {
                                    command.CommandType = CommandType.Text;
                                    command.CommandText = executeCommand;
                                    command.ExecuteNonQuery();
                                }
                                if (connection.State == ConnectionState.Open)
                                {
                                    connection.Close();
                                    connection.Dispose();
                                }
                            }
                        }
                    }
                }
            }
            success = true;

            return success;
        }

        #endregion ExexuteBatch


        //protected Tuple GetMultipleRecord<Tuple>(string CommandText, DynamicParameters CommandParameters, CommandType commandType, IDbTransaction transaction = null)
        //{
        //    Tuple scalarValue = new Tuple();
        //    try
        //    {
        //        using (IDbConnection connection = GetConnection())
        //        {
        //            connection.Open();
        //            var result = connection.Query<T>(CommandText, CommandParameters, commandType: commandType, transaction: transaction);
        //            if (result != null && result.Count() > 0)
        //                scalarValue = result.Single();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return scalarValue;
        //}
    }

}

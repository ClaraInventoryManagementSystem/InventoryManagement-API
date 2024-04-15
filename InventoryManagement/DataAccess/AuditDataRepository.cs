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

namespace InventoryManagement.DataAccess
{
    public class AuditDataRepository : DataAccessRepository<object>, IAuditDataRepository
    {

        public AuditDataRepository(IConfiguration config) : base(config)
        {

        }


        
           

        private bool _BuildDetailstable(MySqlDataReader reader, int nRole, string tablename, out TableData tableData)
        {
            tableData = null;
            IEnumerable<TableColumnData> columnsList = UIColumnCache.Instance.GetScreenColumnsbyRole(5, nRole);
            if (reader != null)
            {
                // Build schema columns
                List<string> SchemaColumns = new List<string>();
                DataTable dt = reader.GetSchemaTable();
                if (dt != null)
                {
                    foreach (DataRow r in dt.Rows)
                    {

                        SchemaColumns.Add(r.Field<String>("ColumnName"));
                    }
                }
                //
                IList<TableColumnData> tableColumns = new List<TableColumnData>();
                foreach (TableColumnData t in columnsList)
                {
                    if (SchemaColumns.Contains(t.FIELD_NAME))
                    {
                        tableColumns.Add(t);
                    }
                }
                IList<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                string strTemp = String.Empty;
                while( reader.Read())
                {
                    Dictionary<string, string> gridRow = new Dictionary<string, string>();
                    foreach (TableColumnData tc in tableColumns)
                    {
                        gridRow.Add(tc.FIELD_NAME, reader[tc.FIELD_NAME].ToString());
                    }
                    rows.Add(gridRow);
                }
                tableData = new TableData();
                tableData.Name = tablename;
                tableData.Columns = tableColumns;
                tableData.Rows = rows;
                return true;
            }
            return false;
        }

        private bool _BuildAuditData(MySqlDataReader reader, int nRole, string tablename, out TableData tableData)
        {
            tableData = null;
            IEnumerable<TableColumnData> columnsList = UIColumnCache.Instance.GetScreenColumnsbyRole(6, nRole);
            if (reader != null)
            {
                // Build schema columns
                List<string> SchemaColumns = new List<string>();
                DataTable dt = reader.GetSchemaTable();
                if (dt != null)
                {
                    foreach (DataRow r in dt.Rows)
                    {

                        SchemaColumns.Add(r.Field<String>("ColumnName"));
                    }
                }
                //
                IList<TableColumnData> tableColumns = new List<TableColumnData>();
                foreach (TableColumnData t in columnsList)
                {
                    if (SchemaColumns.Contains(t.FIELD_NAME))
                    {
                        tableColumns.Add(t);
                    }
                }
                IList<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
                string strTemp = String.Empty;
                while (reader.Read())
                {
                    Dictionary<string, string> gridRow = new Dictionary<string, string>();
                    foreach (TableColumnData tc in tableColumns)
                    {
                        gridRow.Add(tc.FIELD_NAME, reader[tc.FIELD_NAME].ToString());
                    }
                    rows.Add(gridRow);
                }
                tableData = new TableData();
                tableData.Name = tablename;
                tableData.Columns = tableColumns;
                tableData.Rows = rows;
                return true;
            }
            return false;
        }




        public bool AddNewSystemAuditLog(SystemAuditRecord r)
        {
            
            try
            {
                string connString = _Config.GetSection("AppSettings:ConnStringName") != null ?
                _Config.GetSection("AppSettings:ConnStringName").Value : null;

                MySqlParameter[] p = new MySqlParameter[6];
                p[0] = new MySqlParameter("@Action", r.ACTION);
                p[1] = new MySqlParameter("@userId", r.USER);
                p[2] = new MySqlParameter("@RoleId", r.ROLE);
                p[3] = new MySqlParameter("@Deescription", r.DESCRIPTION);
                p[4] = new MySqlParameter("@REFERENCE_ID", r.REFERENCE_ID);
                p[5] = new MySqlParameter("@REMARKS", r.REMARKS);

                object o = MySqlHelper.ExecuteScalar(connString, CommandType.Text, DBQueryConstant.ADD_SYSTEM_AUDIT_LOG, p);
                return true;
            }
            catch (Exception eErr)
            {
                Log.Logger.Error(eErr.Message);
                return false;
            }
        }
        public ResponseData GetSampleAudit(int  inwardID, int user, int nRole)
        {
            ResponseData response = new ResponseData();
            IList<TableData> tblList = new List<TableData>();

            try
            {
                
                string connString = _Config.GetSection("AppSettings:ConnStringName") != null ?
                _Config.GetSection("AppSettings:ConnStringName").Value : null;
                MySqlParameter[] sqlParams = new MySqlParameter[1];
                sqlParams[0] = new MySqlParameter("@inward_id", inwardID);
               
                string strSQL = DBQueryConstant.AUDIT_AR_DETAILS  + DBQueryConstant.GET_AUDIT_LOG_BY_SAMPLE;
                MySqlDataReader reader = MySqlHelper.ExecuteReader(connString, CommandType.Text, strSQL, sqlParams);
                if (reader != null)
                {
                    TableData t = null;
                    if (!_BuildDetailstable(reader, nRole, "AR Details", out t))
                    {
                        response.Error = "Failed to load AuditLog";
                        response.DataType = DataResultType.Error;
                        response.ErrorNumber = 101;
                        return response;
                    }   
                    tblList.Add(t);

                    if (reader.NextResult())
                    {
                        t = null;
                        if (!_BuildAuditData(reader, nRole, "Audit Details", out t))
                        {
                            response.Error = "Failed to load AuditLog";
                            response.DataType = DataResultType.Error;
                            response.ErrorNumber = 101;
                            return response;
                        }
                            
                        tblList.Add(t);
                    }
                }

                if (tblList.Count > 0 && TableData.HasAnyRows(tblList) )
                {
                    TableListData td = new TableListData();
                    td.Data = tblList;
                    response.DataType = DataResultType.Success;
                    response.Data = td;
                    return response;

                }
                response.DataType = DataResultType.NoData;
                return response;

            }
            catch( Exception eError)
            {
                Log.Logger.Error(eError.Message);
                response.Error = "Server Error";
                response.DataType = DataResultType.Error;
                response.ErrorNumber = 500;
                return response;
            }
            
        }


        public int GetSystemLogTotalCount(string startDate, string endData)
        {
            int RecordCount = 0;
            string connString = _Config.GetSection("AppSettings:ConnStringName") != null ?
                _Config.GetSection("AppSettings:ConnStringName").Value : null;
            string sDate = startDate.ConvertToDate();
            string eDate = endData.ConvertToDate();
            string strSQL = string.Format(DBQueryConstant.GET_SYSTEM_AUDIT_COUNT, sDate, eDate);

           
            object o = MySqlHelper.ExecuteScalar(connString, CommandType.Text, strSQL);
            int.TryParse(o.ToString(), out RecordCount);
            return RecordCount;

        }

        public ResponseData GetSystemAudit(int userid, int userRole, string startdate, string enddate, int perPage, int pageStart, int pageEnd, string SortBy, string SortOrder)
        {
            ResponseData response = new ResponseData();

            string connString = _Config.GetSection("AppSettings:ConnStringName") != null ?
                _Config.GetSection("AppSettings:ConnStringName").Value : null;

            string sDate = startdate.ConvertToDate();
            string eDate = enddate.ConvertToDate();

            if (perPage == 0) perPage = DBQueryConstant.DefaultPageSize;

            MySqlParameter[] sqlParams = new MySqlParameter[4];
            sqlParams[0] = new MySqlParameter("@lowerdate", sDate);
            sqlParams[1] = new MySqlParameter("@upperdate", eDate);
            sqlParams[2] = new MySqlParameter("@perPage", perPage);
            sqlParams[3] = new MySqlParameter("@pageStart", pageStart>0? pageStart-1:0);

            IList<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
            TableData t = new TableData();
            t.Columns = UIColumnCache.Instance.GetScreenColumnsbyRole(8, userRole);
            MySqlDataReader reader = MySqlHelper.ExecuteReader(connString, CommandType.Text, DBQueryConstant.GET_SYSTEM_AUDIT_LOG, sqlParams);
            if (reader != null)
            {
                while( reader.Read())
                {
                    Dictionary<string, string> Row = new Dictionary<string, string>();

                    foreach (TableColumnData td in t.Columns)
                    {
                        if (!Row.ContainsKey(td.FIELD_NAME))
                            Row.Add(td.FIELD_NAME, reader[td.FIELD_NAME].ToString());
                    }
                    rows.Add(Row);

                }
                t.Rows = rows;
                
            }
            else
            {
                response.DataType = DataResultType.NoData;
                return response;
            }
            response.DataType = DataResultType.Success;
            response.Data = t;
            return response;
        }
    }
}

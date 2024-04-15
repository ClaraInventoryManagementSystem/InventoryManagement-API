using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class TableColumnData
    {

        public TableColumnData()
        { }
        public TableColumnData(TableColumnData refernce)
        {
            // [JsonProperty(PropertyName ="columnindex")]
            ID = refernce.ID;
            DISPLAY_NAME = refernce.DISPLAY_NAME;
            FIELD_NAME = refernce.FIELD_NAME;
            WIDTH = refernce.WIDTH;
            SORT = refernce.SORT;
            DATA_TYPE = refernce.DATA_TYPE;
            COLUMN_TYPE = refernce.COLUMN_TYPE;
            SCREEN_ID = refernce.SCREEN_ID;
            ROLE_ACCESS = refernce.ROLE_ACCESS;
            CAN_EDIT = refernce.CAN_EDIT;
            SEARCH_COLUMN = refernce.SEARCH_COLUMN;
            //Status = refernce.Status;
            
        }


        // To be read from Database
        // Cretae a Table 
        // DISPLAY_NAME,
        // COLUMN_NAME
        // WIDTH
        // SORT_ORDER
        // DATA_TYPE : For Server side processing
        // COLUM_TYPE : for UI
        // COLUMN_CATEGORY : FOR GRID  / DETAIL ( 1 for Grid, 2 details )

        /*

        private static readonly  int g_nColumnCount = 66;
       
        public static string[,] _ArColumnList = new string[66, 7] {
        { "AR Number","AR_NUMBER","100","asc","0","0","1" },
        { "AR Type","AR_TYPE","100","asc","1","0","1" },
        { "Registred Date","REGISTRED_DATE","100","asc","0","0","1" },
        { "Customer Name","CUSTOMER_SHORT_NAME","100","asc","0","0","1" },
        { "Location Name","LOCATION_SHORT_NAME","100","asc","0","0","1" },
        { "Conact Name","CONTACT_PERSON","100","asc" ,"0","0","1"},
        { "AR Status","AR_STATUS","100","asc","1","0","1" },
        { "AR Category","AR_CATEGORY","100","asc" ,"1","0","1"},
        { "Ar Sub Category","AR_SUB_CATEGORY","100","asc","1","0","1" },
        { "PO Number","PO_NUMBER","100","asc","0" ,"0","1"},
        { "Other Customer ID","OTHER_CUSTOMER_ID","100","asc","0","0","1" },
        { "Invoice Amount","INVOICE_AMOUNT","100","asc","0","0","1" },
        { "Is Rush","IS_RUSH","100","asc","0","0","1" },
        { "Created By","CREATE_BY_USER","100","asc","0","0","1" },
        { "Modified By","MODIFIED_BY_USER","100","asc","1" ,"0","1"},
        { "Modified Date","LAST_MODIFIED_DATE","100","asc","0","0","1" },
        { "Material Name","MATERIAL_SHORT_NAME","100","asc","0" ,"0","1"},
        { "Condiation","MATERIAL_CONDIATION","100","asc","1" ,"0","1"},
        { "Condiation Remarks","MATERIAL_CONDITION_REMARKS","100","asc","0","0","1" },
        { "Retun Sample","RETURN_MATERIAL","100","asc","0" ,"0","1"},
        { "Test Method","TEST_METHOD","100","asc","1","0","1" },
        { "Test Method Ref","TEST_METHOD_REF","100","asc","1" ,"0","1"},
        { "Material Type","MATERIAL_TYPE","100","asc","1","1","1" },
        { "Material Category","MATERIAL_CATEGORY","100","asc","1" ,"0","1"},
        { "Controlled Substance","CONTROLLED_SUBSTANCE","100","asc","0","0","1" },
        { "Disposal Type","DISPOSABLE_TYPE","100","asc","1" ,"0","1"},
        { "Numer of Lots","NUMBER_OF_LOTS","100","asc","0","0","1" },
        { "Tests Count","TOTAL_TESTS_COUNT","100","asc","0" ,"0","1"},
        { "AR REMARKS","REMARKS","100","asc","0" ,"0","2"},
        { "Sample Remarks","MATERIAL_REMARKS","100","asc","0" ,"0","2"},
        { "Category Remarks","AR_CATEGORY_REMARKS","100","asc","0" ,"0","2"},
        { "Customer Name","CUSTOMER_LONG_NAME","100","asc","0" ,"0","2"},
        { "Location Name","LOCATION_LONG_NAME","100","asc","0" ,"0","2"},
        { "Address 1","STREET_ADDRESS1","100","asc","0" ,"0","2"},
        { "Address 2","STREET_ADDRESS2","100","asc","0" ,"0","2"},
        { "City","CITY","100","asc","0" ,"0","2"},
        { "State","STATE","100","asc","0" ,"0","2"},
        { "Country","COUNTRY","100","asc","0" ,"0","2"},
        { "Zip code","ZIP_CODE","100","asc","0" ,"0","2"},
        { "Conact Person","CONTACT_PERSON","100","asc","0" ,"0","2"},
        { "Conact Email","CONTACT_EMAIL","100","asc","0" ,"0","2"},
        { "Conact Phone","CONTACT_PHONE","100","asc","0" ,"0","2"},
        { "Conact Phone Ext","CONTACT_PHONE_EXT","100","asc","0" ,"0","2"},
        { "Lot Number","CUSTOMER_LOT_NO","100","asc","0" ,"0","2"},
        { "Material Code 1","CUSTOMER_MAT_CODE1","100","asc","0" ,"0","2"},
        { "Material Code 2","CUSTOMER_MAT_CODE2","100","asc","0" ,"0","2"},
        { "Container Type","CONTAINER_TYPE","100","asc","1" ,"0","2"},
        {"Number of Containers","NO_OF_CONTAINERS","100","asc","0" ,"0","2"},
        {"Quantity Received","QTY_RECEIEVED","100","asc","0" ,"0","2"},
        {"Quantity UOM","QTY_UOM","100","asc","1" ,"0","2"},
        {"Storage Method","STORAGE_METHOD","100","asc","1" ,"0","2"},
        {"Test Name","TEST_SHORT_NAME","100","asc","0" ,"0","2"},
        {"Test Type","TEST_TYPE","100","asc","1" ,"0","2"},
        {"Test Status","TEST_STATUS","100","asc","1" ,"0","2"},
        {"Test Assigned TO","ASSIGN_TO","100","asc","0" ,"0","2"},
        {"Test Results Submit By","RESULTS_SUBMITTED_BY","100","asc","0" ,"0","2"},
        {"Test Remarks","TEST_REMARKS","100","asc","0" ,"0","2"},
        
        {"Test Name","TEST_SHORT_NAME","100","asc","0" ,"0","3"},
        {"Test Type","TEST_TYPE","100","asc","1" ,"0","3"},
        {"Test Status","TEST_STATUS","100","asc","1" ,"0","3"},
        {"LOT NO","LOT_NO","100","asc","0" ,"0","3"},
        {"AR NUMBER","AR_NUMBER","100","asc","0" ,"0","3"},
        {"TEST STATUS","TEST_STATUS","100","asc","0" ,"0","3"},       
        {"MATERIAL Name","MATERIAL_Name","100","asc","0" ,"0","3"},
        {"IR TEST ID","IR_TEST_ID","100","asc","0" ,"0","3"},
        {"Test Assigned TO","ASSIGN_TO","100","asc","0" ,"0","3"},

        };

       */

        [JsonIgnore]
        public int CAN_EDIT { get; set; }


       // [JsonProperty(PropertyName ="columnindex")]
        [JsonIgnore]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "displayname")]
        public string DISPLAY_NAME { get; set; }

        [JsonProperty(PropertyName = "fieldname")]
        public string FIELD_NAME { get; set; }

        [JsonProperty(PropertyName = "width")]
        public int WIDTH { get; set; }

        [JsonProperty(PropertyName = "sort")]
        public string SORT { get; set; }


       // [JsonProperty(PropertyName = "datatype")]
        [JsonIgnore]
        public int DATA_TYPE { get; set; } // for backend user

        [JsonProperty(PropertyName = "columntype")]
        public string COLUMN_TYPE { get; set; } // FOr navigation

        //[JsonProperty(PropertyName = "screenid")]
        [JsonIgnore]
        public int SCREEN_ID { get; set; }
       

        [JsonIgnore]
        public int ROLE_ACCESS { get; set; }

        [JsonProperty(PropertyName = "SearchName")]
        public string SEARCH_COLUMN { get; set; }

       // [JsonProperty(PropertyName = "Status")]
       // public string Status { get; set; }
        

        //// This function needs to moved to access layer once the DB table is defined
        //public static IEnumerable<TableColumnData> GetGridColumns(int nRole = 1)
        //{
        //    IList<TableColumnData> columns = new List<TableColumnData>();
        //    int nIndex = 1;
        //    for( int i=0;i< g_nColumnCount; i++)
        //    {
        //        if(_ArColumnList[i, 6] == "1")
        //        {
        //            TableColumnData column = new TableColumnData();
        //            /*
        //            column.ColumnIndex = nIndex.ToString();
        //            nIndex++;
        //            column.DisplayName = _ArColumnList[i, 0];
        //            column.FieldName = _ArColumnList[i, 1];
        //            column.Width = _ArColumnList[i, 2];
        //            column.Sort = _ArColumnList[i, 3];
        //            column.DataType = _ArColumnList[i, 4];
        //            column.ColumnType = _ArColumnList[i, 5];
        //            column.ColumnCategory = _ArColumnList[i, 6];
        //            */
        //            columns.Add(column);
        //        }

        //    }
        //    return columns;
        //}

        //public static IEnumerable<TableColumnData> GetInfoColumns(int nRole = 1)
        //{
        //    IList<TableColumnData> columns = new List<TableColumnData>();
        //    int nIndex = 1;
        //    for (int i = 0; i < g_nColumnCount; i++)
        //    {
        //        if (_ArColumnList[i, 6] == "2")
        //        {
        //            TableColumnData column = new TableColumnData();
        //            /*
        //            column.ColumnIndex = nIndex++.ToString();
        //            column.DisplayName = _ArColumnList[i, 0];
        //            column.FieldName = _ArColumnList[i, 1];
        //            column.Width = _ArColumnList[i, 2];
        //            column.Sort = _ArColumnList[i, 3];
        //            column.DataType = _ArColumnList[i, 4];
        //            column.ColumnType = _ArColumnList[i, 5];
        //            column.ColumnCategory = _ArColumnList[i, 6];
        //            */
        //            columns.Add(column);
        //        }

        //    }
        //    return columns;
        //}

        //public static IEnumerable<TableColumnData> GetPendingTest(int nRole = 1) 
        //{
        //    IList<TableColumnData> columns = new List<TableColumnData>();
        //    int nIndex = 1;
        //    for (int i = 0; i < g_nColumnCount; i++)
        //    {
        //        if (_ArColumnList[i, 6] == "3")
        //        {
        //            TableColumnData column = new TableColumnData();
        //            /*
        //            column.ColumnIndex = nIndex++.ToString();
        //            column.DisplayName = _ArColumnList[i, 0];
        //            column.FieldName = _ArColumnList[i, 1];
        //            column.Width = _ArColumnList[i, 2];
        //            column.Sort = _ArColumnList[i, 3];
        //            column.DataType = _ArColumnList[i, 4];
        //            column.ColumnType = _ArColumnList[i, 5];
        //            column.ColumnCategory = _ArColumnList[i, 6];
        //            */
        //            columns.Add(column);
        //        }

        //    }
        //    return columns;
        //}
    }



    
}

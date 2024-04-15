using System;
namespace InventoryManagement.Common
{
    public class ResponseData
    {
        public ResponseData()
        {
        }
        public ResponseData(DataResultType type,object data)
        {
            DataType = type;
            Data = data;
        }
        public ResponseData( DataResultType t,string error, int errorNumber)
        {
            DataType = t;
            Error = error;
            ErrorNumber = errorNumber;

        }

       public DataResultType DataType { get; set; }
       public string Error { get; set; }
       public int ErrorNumber { get; set; }
       public object Data { get; set; }


    }
}

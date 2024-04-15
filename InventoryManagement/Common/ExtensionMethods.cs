using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace InventoryManagement
{
    public static  class ExtensionMethods
    {

        public static string ConvertToDate(this string input)
        {
            try
            {
                if(input != null && input != String.Empty)
                {
                    DateTime dt = DateTime.Parse(input);
                    return dt.ToString("yyyy-MM-dd");
                }
                

            }
            catch( Exception eErr)
            {
                Log.Error(input);
                Log.Error(eErr.Message);
            }
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public static string ConvertToDateMMDDYYY(this string input)
        {
            try
            {
                if (input != null && input != String.Empty)
                {
                    DateTime dt = DateTime.Parse(input);
                    return dt.ToString("MM-dd-yyyy");
                }


            }
            catch (Exception eErr)
            {
                Log.Error(input);
                Log.Error(eErr.Message);
            }
            return DateTime.Now.ToString("MM-dd-yyyy");
        }
    }
}

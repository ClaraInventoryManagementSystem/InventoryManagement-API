using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class ApiErrorResponse
    {

        public ApiErrorResponse(string error, int number)
        {
            ErrorMessage = error;
            ErrorCode = number;

        }

        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public string Status = "error";
    }
}

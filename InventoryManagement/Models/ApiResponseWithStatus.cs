using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class ApiResponseWithStatus
    {
        public bool Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}

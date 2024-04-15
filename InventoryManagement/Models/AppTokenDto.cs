using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class AppTokenDto
    {
        public int Token_ID { get; set; }
        public int User_Id { get; set; }
        public string AuthToken { get; set; }
        public DateTime Issued_On { get; set; }
        public DateTime Expired_On { get; set; }
    }
}

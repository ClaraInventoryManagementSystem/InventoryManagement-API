using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class UserData
    {
        //AppUser
        public User AppUser { get; set; }
        public string AuthToken { get; set; }
        public DateTime TokenExpiresOn { get; set; }
        public int TokenID { get; set; }
        public int UserID { get; set; }

        public int RoleID { get; set; }
    }
}

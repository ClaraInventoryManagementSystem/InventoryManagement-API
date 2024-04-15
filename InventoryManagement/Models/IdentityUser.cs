using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class IdentityUser : BaseModel
    {

        //public AppUser userDetails { get; set; }
        public User userDetails { get; set; }
        public string AuthToken { get; set; }
        public string Role_Code { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}

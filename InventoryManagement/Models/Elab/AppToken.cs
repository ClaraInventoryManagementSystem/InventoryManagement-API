using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.Models
{
    public class AppToken
    {
        public int TokenID { get; set; }
        public int UserId { get; set; }
        public string AuthToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }

        public virtual AppUser TokenNavigation { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace InventoryManagement.Models
{
    public partial class AppRole
    {
        public AppRole()
        {
            AppRoleUser = new HashSet<AppRoleUser>();
        }

        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public string RoleDisplayName { get; set; }
        public DateTime? RoleStartDate { get; set; }
        public DateTime? RoleEndDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<AppRoleUser> AppRoleUser { get; set; }
    }
}

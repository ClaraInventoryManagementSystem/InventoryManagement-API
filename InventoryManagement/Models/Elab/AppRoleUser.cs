using System;
using System.Collections.Generic;

namespace InventoryManagement.Models
{
    public partial class AppRoleUser
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? Role { get; set; }
        public string RoleName { get; set;}      
    }
}

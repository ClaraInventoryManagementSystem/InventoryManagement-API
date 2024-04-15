using System;
using System.Collections.Generic;

namespace InventoryManagement.Models
{
    public partial class AppUser
    {
        public AppUser()
        {
            AppRoleUser = new HashSet<AppRoleUser>();

            AppToken = new HashSet<AppToken>();
        }

        public int User_Id { get; set; }
        public string USER_FIRST_NAME { get; set; }
        public string USER_LAST_NAME { get; set; }
        public int? LDAP_USER_DESCRIPTOR { get; set; }
        public string ENCRYPTED_PASSWORD { get; set; }
        public string ENCRYPTED_PIN { get; set; }
        public int? LDAP_USER_ID { get; set; }
        public int? FAILED_LOGIN_ATTEMPTS { get; set; }
        //public string UserEmailId { get; set; }
        public string USER_EMAIL_ID { get; set; }
        public int? Locked { get; set; }
        public DateTime? LockedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public int ROLE_ID { get; set; }
        public string ROLE_CODE { get; set; }

        public virtual ICollection<AppRoleUser> AppRoleUser { get; set; }
        public virtual ICollection<AppToken> AppToken { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace InventoryManagement.Models
{
    public class User
    {
        // id, firstname, lastname, login, password, role, lastlogin, active, roleid, rolename
        public User()
        {
            AppRoleUser = new HashSet<AppRoleUser>();

            AppToken = new HashSet<AppToken>();
        }

        //public int id { get; set; }
        //public string firstname { get; set; }
        //public string lastname { get; set; }
        //public string password { get; set; }
        //public string login { get; set; }
        //public string role { get; set; }
        //public DateTime lastlogin { get; set; }
        //public int active { get; set; }

        public int roleid { get; set; }
        public string rolename { get; set; }



        public virtual int Id { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string LogIn { get; set; }

        public virtual string Password { get; set; }

        public virtual string Role { get; set; }

        public virtual DateTime? LastLogIn { get; set; }

        public virtual bool Active { get; set; }

        public virtual string Email { get; set; }

        public virtual int Phone { get; set; }

        //public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

        public override string ToString()
        {
            return $"{FirstName} {LastName}";
        }

        //public virtual void AddRoles(UserRole r)
        //{
        //    r.User = this;
        //    Roles.Add(r);

        //}

        //public virtual void RemoveRole(UserRole r)
        //{

        //    Roles.Remove(r);

        //}

        public string ENCRYPTED_PASSWORD { get; set; }

        public virtual ICollection<AppRoleUser> AppRoleUser { get; set; }
        public virtual ICollection<AppToken> AppToken { get; set; }

    }
}

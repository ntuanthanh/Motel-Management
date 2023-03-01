using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleFeatures = new HashSet<RoleFeature>();
            RoleUsers = new HashSet<RoleUser>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<RoleFeature> RoleFeatures { get; set; }
        public virtual ICollection<RoleUser> RoleUsers { get; set; }
    }
}

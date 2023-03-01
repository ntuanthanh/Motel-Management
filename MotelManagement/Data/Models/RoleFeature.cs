using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class RoleFeature
    {
        public int FeatureId { get; set; }
        public int RoleId { get; set; }

        public virtual Feature Feature { get; set; }
        public virtual Role Role { get; set; }
    }
}

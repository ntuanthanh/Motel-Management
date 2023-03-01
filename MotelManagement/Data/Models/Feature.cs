using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Feature
    {
        public Feature()
        {
            RoleFeatures = new HashSet<RoleFeature>();
        }

        public int FeatureId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual ICollection<RoleFeature> RoleFeatures { get; set; }
    }
}

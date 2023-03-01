using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Contract
    {
        public int ContractId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime EffectiveFrom { get; set; }
        public DateTime EffectiveTo { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}

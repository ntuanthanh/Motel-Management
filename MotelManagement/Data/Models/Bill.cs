using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Bill
    {
        public int BillId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime PaidTime { get; set; }
        public DateTime AcceptTime { get; set; }
        public decimal RoomBill { get; set; }
        public decimal ElectricBill { get; set; }
        public decimal WaterBill { get; set; }
        public decimal ServiceBill { get; set; }
        public int BillState { get; set; }
        public decimal? Debt { get; set; }
        public string BankingImage { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}

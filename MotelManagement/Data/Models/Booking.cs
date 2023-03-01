using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime? MeetingDate { get; set; }

        public virtual Room Room { get; set; }
        public virtual User User { get; set; }
    }
}

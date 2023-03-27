﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Passing
    {
        public int PassingId { get; set; }
        public int RoomId { get; set; }
        public int? MemberId { get; set; }
        public int UserRequestId { get; set; }
        public int Status { get; set; }
        public DateTime BookingTime { get; set; }
        public DateTime? MeetingDate { get; set; }

        public virtual User Member { get; set; }
        public virtual Room Room { get; set; }
        public virtual User UserRequest { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class RoomType
    {
        public RoomType()
        {
            Rooms = new HashSet<Room>();
        }

        public int RoomTypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}

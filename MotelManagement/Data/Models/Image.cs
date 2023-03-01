using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }
        public string Url { get; set; }
        public int RoomId { get; set; }

        public virtual Room Room { get; set; }
    }
}

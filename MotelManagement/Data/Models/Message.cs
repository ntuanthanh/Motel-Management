using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Message
    {
        public int MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public DateTime TimeSend { get; set; }
        public int RoomId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public int Status { get; set; }

        public virtual User ReceiverNavigation { get; set; }
        public virtual Room Room { get; set; }
        public virtual User SenderNavigation { get; set; }
    }
}

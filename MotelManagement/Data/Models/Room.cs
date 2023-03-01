using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class Room
    {
        public Room()
        {
            Bills = new HashSet<Bill>();
            Bookings = new HashSet<Booking>();
            Contracts = new HashSet<Contract>();
            Images = new HashSet<Image>();
            Messages = new HashSet<Message>();
            Passings = new HashSet<Passing>();
        }

        public int RoomId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int MaxPerson { get; set; }
        public int StatusId { get; set; }
        public int RoomTypeId { get; set; }

        public virtual RoomType RoomType { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public virtual ICollection<Passing> Passings { get; set; }
    }
}

using System;
using System.Collections.Generic;

#nullable disable

namespace MotelManagement.Data.Models
{
    public partial class User
    {
        public User()
        {
            Bills = new HashSet<Bill>();
            Bookings = new HashSet<Booking>();
            Contracts = new HashSet<Contract>();
            MessageReceiverNavigations = new HashSet<Message>();
            MessageSenderNavigations = new HashSet<Message>();
            PassingMembers = new HashSet<Passing>();
            PassingUserRequests = new HashSet<Passing>();
            RoleUsers = new HashSet<RoleUser>();
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool Gender { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
        public string RoleName { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Message> MessageReceiverNavigations { get; set; }
        public virtual ICollection<Message> MessageSenderNavigations { get; set; }
        public virtual ICollection<Passing> PassingMembers { get; set; }
        public virtual ICollection<Passing> PassingUserRequests { get; set; }
        public virtual ICollection<RoleUser> RoleUsers { get; set; }
    }
}

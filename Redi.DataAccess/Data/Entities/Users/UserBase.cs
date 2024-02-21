﻿using Microsoft.AspNetCore.Identity;

namespace Redi.DataAccess.Data.Entities.Users
{
    public class UserBase : IdentityUser
    {
        public UserBase() { }
        public UserBase(string userName) : base(userName) { }
        public string? Picture { get; set; }
        public ICollection<Delivery> Deliveries { get; set; } = new HashSet<Delivery>();
        public ICollection<Chat> Chats { get; set; } = new HashSet<Chat>();
    }
}

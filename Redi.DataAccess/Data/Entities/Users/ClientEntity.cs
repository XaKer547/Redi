﻿namespace Redi.DataAccess.Data.Entities.Users
{
    public class ClientEntity : UserBase
    {
        public ClientEntity() { }

        public ClientEntity(string userName) : base(userName) { }

        public float Balance { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
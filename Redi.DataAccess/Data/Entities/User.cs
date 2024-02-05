using Microsoft.AspNetCore.Identity;

namespace Redi.DataAccess.Data.Entities
{
    public class User : IdentityUser
    {
        public User(string userName) : base(userName) { }
        public User() { }


        public float Balance { get; set; }
        public string? Picture { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();
        public ICollection<Card> Cards { get; set; } = new HashSet<Card>();
    }
}

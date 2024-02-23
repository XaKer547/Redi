namespace Redi.Domain.Models.Account
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public float Money { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}

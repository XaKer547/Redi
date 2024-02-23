namespace Redi.Domain.Models.Account
{
    public class WalletInfoDTO
    {
        public double Balance { get; set; }
        public IReadOnlyCollection<TransactionDTO> Transactions { get; set; }
    }
}

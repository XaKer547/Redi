using Redi.Domain.Models.Account;

namespace Redi.Deliverer.Models
{
    public class AuthorizationViewModel
    {
        public IReadOnlyCollection<DelivererPreview> Deliverers { get; set; }
        public SignInDTO SignInDTO { get; set; }
    }
}

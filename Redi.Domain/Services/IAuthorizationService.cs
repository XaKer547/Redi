using Redi.Domain.Models.Account;

namespace Redi.Domain.Services
{
    public interface IAuthorizationService
    {
        Task SignUpAsync(SignUpDTO signUp);
        Task<AuthorizationResult> SingInAsync(SignInDTO signIn);
    }
}

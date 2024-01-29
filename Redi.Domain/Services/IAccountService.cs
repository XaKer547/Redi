using Redi.Domain.Models.Account;

namespace Redi.Domain.Services
{
    public interface IAccountService
    {
        Task RequestPasswordRecoveryAsync(PasswordRecoveryRequestDTO request);
        Task<bool> VerifyOtpCodeAsync(OtpVerificationDTO otpVerification);
        Task ChangePasswordAsync(int userId, string newPassword);

        Task<bool> VerifyEmailExist(string email);
    }
}

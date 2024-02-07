using Redi.Api.Models;

namespace Redi.Api.Infrastructure.Interfaces
{
    public interface IMailService
    {
        Task SendOtpCodeAsync(string email, string code, PasswordRevoveryInfo requestInfo);
    }
}

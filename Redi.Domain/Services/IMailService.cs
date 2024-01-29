namespace Redi.Domain.Services
{
    public interface IMailService
    {
        Task SendOtpCode(string email);
    }
}

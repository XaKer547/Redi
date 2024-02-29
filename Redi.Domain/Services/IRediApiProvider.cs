using Redi.Domain.Models.Account;

namespace Redi.Domain.Services
{
    public interface IRediApiProvider
    {
        Task<T> Get<T>(string url);
        Task Post(string url, object dto);
        Task<T> Post<T>(string url, object dto);
        Task<T> Put<T>(string url, object dto);
        Task<T> Patch<T>(string url, object dto);
        Task Patch(string url, object dto);
        Task Head(string url);
        Task<AuthorizationResult> Authorize(SignInDTO dto);
    }
}

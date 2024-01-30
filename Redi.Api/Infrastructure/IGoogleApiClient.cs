namespace Redi.Api.Infrastructure
{
    public interface IGoogleApiClient
    {
        Task GetUserData(string userToken);
    }
}

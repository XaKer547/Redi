namespace Redi.Api.Infrastructure
{
    public class GoogleApiClient
    {
        private readonly HttpClient _client;

        public GoogleApiClient()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri("https://www.googleapis.com/oauth2/v2")
            };
        }

        public async Task GetUserDataAsync(string userToken)
        {
            var response = await _client.GetAsync($"/userinfo?access_token={userToken}");
        }
    }
}
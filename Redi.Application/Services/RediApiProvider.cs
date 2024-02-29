using Redi.Domain.Models.Account;
using Redi.Domain.Services;
using System.Net.Http.Json;
using System.Text.Json;

namespace Redi.Application.Services
{
    public class RediApiProvider : IRediApiProvider
    {
        private readonly HttpClient _client;
        public RediApiProvider(HttpClient client)
        {
            _client = client;
        }

        public async Task<AuthorizationResult> Authorize(SignInDTO dto)
        {
            var response = await _client.PostAsJsonAsync("api/Authorization/SignIn", dto);

            if (!response.IsSuccessStatusCode)
            {
                return new AuthorizationResult()
                {
                    Success = false,
                };
            }

            var token = await response.Content.ReadAsStringAsync();

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return new AuthorizationResult()
            {
                Success = true,
                Token = token
            };
        }

        public async Task<T> Get<T>(string url)
        {
            var response = await _client.GetAsync(url);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task Head(string url)
        {
            var method = new HttpRequestMessage(HttpMethod.Head, url);

            await _client.SendAsync(method);
        }

        public async Task<T> Patch<T>(string url, object dto)
        {
            var json = JsonSerializer.Serialize(dto);

            var content = new StringContent(json);

            var response = await _client.PatchAsync(url, content);

            var result = await response.Content.ReadFromJsonAsync<T>();

            return result;
        }
        public async Task Patch(string url, object dto)
        {
            var json = JsonSerializer.Serialize(dto);

            var content = new StringContent(json);

            await _client.PatchAsync(url, content);
        }

        public async Task<T> Post<T>(string url, object dto)
        {
            var response = await _client.PostAsJsonAsync(url, dto);

            var result = await response.Content.ReadFromJsonAsync<T>();

            return result;
        }
        public async Task Post(string url, object dto)
        {
            await _client.PostAsJsonAsync(url, dto);
        }

        public async Task<T> Put<T>(string url, object dto)
        {
            var response = await _client.PostAsJsonAsync(url, dto);

            var result = await response.Content.ReadFromJsonAsync<T>();

            return result;
        }
    }
}

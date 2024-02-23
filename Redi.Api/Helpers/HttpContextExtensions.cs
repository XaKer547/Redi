using Redi.Api.Models;

namespace Redi.Api.Helpers
{
    public static class HttpContextExtensions
    {
        private static readonly HttpClient _client = new();
        public static async Task<RequestInfo> GetRequestInfo(this HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"];
            var uaParser = UAParser.Parser.GetDefault();
            var c = uaParser.Parse(userAgent);

            var os = c.OS.ToString();
            var browser = c.UA.ToString();

            var remoteAdress = context.Request.HttpContext.Connection.RemoteIpAddress?.ToString();

            var ip = remoteAdress != "::1" ? remoteAdress : "87.76.9.110";

            var info = await _client.GetFromJsonAsync<IpInfo>($"http://ip-api.com/json/{ip}");

            return new RequestInfo()
            {
                Browser = browser,
                Device = os,
                Ip = ip,
                City = info.City,
                Country = info.Country,
            };
        }
    }
}

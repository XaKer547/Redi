using System.Text.Json.Serialization;

namespace Redi.Api.Infrastructure
{
    public class SMTPConfiguration
    {
        public string Email { get; set; }

        public string Password { get; set; }

        [JsonPropertyName("SMTP")]
        public string Smtp { get; set; }

        public int Port { get; set; }
    }
}

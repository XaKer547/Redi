using Redi.Api.Infrastructure.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Redi.Api.Infrastructure
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly SMTPConfiguration _sMTPConfiguration = new SMTPConfiguration();

        public MailService(
            IConfiguration configuration,
            ILogger<MailService> logger)
        {

            configuration.GetSection("Authentication:Mail").Bind(_sMTPConfiguration);

            _logger = logger;
        }

        public async Task SendOtpCodeAsync(string email, string code)
        {
            SmtpClient client = new()
            {
                EnableSsl = true,
                Host = _sMTPConfiguration.Smtp,
                Port = _sMTPConfiguration.Port,
                UseDefaultCredentials = false,
                DeliveryFormat = SmtpDeliveryFormat.International,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(_sMTPConfiguration.Email, _sMTPConfiguration.Password)
            };

            using MailMessage msg = new()
            {
                From = new MailAddress(_sMTPConfiguration.Email),
                Subject = "Востановление пароля",
                IsBodyHtml = true,
                Body = string.Format("<html><head></head><body><b>Test HTML Email</b></body>")
            };

            msg.To.Add(new MailAddress(email));

            try
            {
                await client.SendMailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "При отправке сообщения произошла ошибка!");
            }
        }
    }
}

using Redi.Api.Infrastructure.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Redi.Api.Infrastructure
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _email;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _email = _configuration["Authentication:Google:Email"];
        }

        public async Task SendOtpCodeAsync(string email, string code)
        {
            SmtpClient client = new()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
                Host = "smtp.gmail.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _configuration["Authentication:Google:Password"])
            };

            using MailMessage msg = new()
            {
                From = new MailAddress(_email),
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
            }
        }
    }
}

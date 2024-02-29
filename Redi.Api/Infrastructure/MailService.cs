using Redi.Api.Infrastructure.Interfaces;
using Redi.Api.Models;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace Redi.Api.Infrastructure
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly SMTPConfiguration _sMTPConfiguration = new();
        private readonly IHttpContextAccessor _httpAccessor;
        public MailService(
            IConfiguration configuration,
            ILogger<MailService> logger,
            IHttpContextAccessor httpAccessor)
        {
            configuration.GetSection("Authentication:Mail").Bind(_sMTPConfiguration);

            _logger = logger;
            _httpAccessor = httpAccessor;
        }

        public async Task SendOtpCodeAsync(string email, string code, PasswordRevoveryInfo requestInfo)
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

            var imgPath = $"http://{_httpAccessor.HttpContext?.Request.Host.Value}";

            var mailBody = "<table lang=\"x-class-body\" style=\"border-spacing:0;border-collapse:collapse;vertical-align:top;background:#407BFF;text-align:left;height:100%;width:100%;padding:0;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;color:#222222\" width=\"100%\" valign=\"top\" align=\"left\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left;background-color:#0560FA\" valign=\"top\" align=\"left\" bgcolor=\"#262626\"> <td align=\"center\" valign=\"top\" style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;line-height:19px;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;text-align:center\"> <center style=\"width:100%;min-width:580px\"> <table lang=\"x-class-container\" style=\"border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:inherit;width:580px;margin:0 auto\" width=\"580\" valign=\"top\" align=\"inherit\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left;height:10px\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;line-height:10px\" valign=\"top\" align=\"left\">&nbsp;</td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\"> " +
             $"<img src=\"{imgPath}/assets/redi_logo.svg\" style=\"outline:none;text-decoration:none;width:auto;max-width:100%;float:left;clear:both;display:block;border:none;height:49px\" /> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left;height:10px\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;line-height:10px\" valign=\"top\" align=\"left\">&nbsp;</td> </tr> </tbody> </table> </center> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td align=\"center\" valign=\"top\" height=\"11\" style=\"vertical-align:top;font-size:14px;border-collapse:collapse;padding:0;word-wrap:break-word;line-height:19px;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;text-align:center;height:11px\"> &nbsp; </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td align=\"center\" valign=\"top\" style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;line-height:19px;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;text-align:center\"> <table style=\"border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;background:#fff;text-align:inherit;width:580px;margin:0 auto\" width=\"580\" valign=\"top\" align=\"inherit\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td lang=\"x-class-container__padding\" style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;width:30px\" width=\"30\" valign=\"top\" align=\"left\"></td> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\"> <table style=\"border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left;color:#262626\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\"> <h1 style=\"color:#222222;font-family:Lato,Arial,sans-serif;padding:0;margin:0;line-height:1.3;word-break:normal;font-weight:700;font-size:26px;margin-top:1em;text-align:center\"> Востановление пароля </h1>" +
             $"<img src=\"{imgPath}/assets/forgot_password.svg\" /> <p lang=\"x-small-text-left\" style=\"margin:0;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;padding:0;line-height:24px;font-size:14px;margin-bottom:0;padding-top:20px;text-align:center\">" +
             $"Привет, привет, {requestInfo.UserName}. нам сообщили что вы забыли пароль от своего аккаунта. Высылаем вам код для сброса пароля. </p> <p lang=\"x-small-text-left\" style=\"margin:0;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;padding:0;line-height:24px;font-size:14px;margin-bottom:0;padding-top:20px;text-align:center\"> Код действителен в течение <b>15 минут</b> после получения этого письма. </p> <p style=\"margin:0;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;padding:0;line-height:24px;font-size:14px;margin-bottom:0;padding-top:20px;text-align:center\"> Ваш код безопасности: </p> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;height:15px\" valign=\"top\" align=\"left\"></td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left;border-top:1px solid #e0e0e0\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;color:#EC8000;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;letter-spacing:10px;line-height:2;font-size:48px;text-align:center\" valign=\"top\" align=\"center\">" +
             $"{code} </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left;border-top:1px solid #e0e0e0\" valign=\"top\" align=\"left\"> <td style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;height:20px\" valign=\"top\" align=\"left\"></td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;line-height:19px;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;text-align:center\" valign=\"top\" align=\"center\"> " +
             "</td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;height:20px\" valign=\"top\" align=\"left\"></td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left;border-top:1px solid #e0e0e0\" valign=\"top\" align=\"left\"> <td style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;height:30px\" valign=\"top\" align=\"left\"></td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\"> <table style=\"border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:left;width:100%\" width=\"100%\" valign=\"top\" align=\"left\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td lang=\"x-class-info__item\" style=\"word-wrap:break-word;border-collapse:collapse;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;padding:0 10px;width:25%;font-size:12px;text-align:center\" width=\"25%\" valign=\"top\" align=\"center\"> IP-адрес: <div lang=\"x-class-info__data\" style=\"font-weight:bold\">" +
             $"{requestInfo.RequestInfo.Ip} </div> </td> <td lang=\"x-class-info__item\" style=\"word-wrap:break-word;border-collapse:collapse;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;padding:0 10px;width:25%;font-size:12px;text-align:center\" width=\"25%\" valign=\"top\" align=\"center\"> OS: <div lang=\"x-class-info__data\" style=\"font-weight:bold\"> " +
             $"{requestInfo.RequestInfo.Device} </div> </td> <td lang=\"x-class-info__item\" style=\"word-wrap:break-word;border-collapse:collapse;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;padding:0 10px;width:25%;font-size:12px;text-align:center\" width=\"25%\" valign=\"top\" align=\"center\"> Браузер: <div lang=\"x-class-info__data\" style=\"font-weight:bold\">" +
             $"{requestInfo.RequestInfo.Browser} </div> </td> <td lang=\"x-class-info__item\" style=\"word-wrap:break-word;border-collapse:collapse;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;padding:0 10px;width:25%;font-size:12px;text-align:center\" width=\"25%\" valign=\"top\" align=\"center\"> Предполагаемое место: <div lang=\"x-class-info__data\" style=\"font-weight:bold\">" +
             $"{requestInfo.RequestInfo.Location} </div> </td> </tr> </tbody> </table> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;height:30px\" valign=\"top\" align=\"left\"></td> </tr> </tbody> </table> </td> <td lang=\"x-class-container__padding\" style=\"text-align:left;word-wrap:break-word;border-collapse:collapse;padding:0;vertical-align:top;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px;width:30px\" width=\"30\" valign=\"top\" align=\"left\"></td> </tr> </tbody> </table> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td align=\"center\" valign=\"top\" style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;line-height:19px;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;text-align:center\"> <center style=\"width:100%;min-width:580px\"> <table lang=\"x-class-container\" style=\"border-spacing:0;border-collapse:collapse;padding:0;vertical-align:top;text-align:inherit;width:580px;margin:0 auto\" width=\"580\" valign=\"top\" align=\"inherit\"> <tbody> <tr style=\"padding:0;vertical-align:top;text-align:left;height:30px\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\">&nbsp;</td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;line-height:19px;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;font-size:14px;text-align:center\" valign=\"top\" align=\"center\"> <p style=\"margin:0;font-family:Lato,Arial,sans-serif;font-weight:normal;padding:0;margin-bottom:10px;color:#e0e0e0;line-height:1.3;font-size:10px;text-align:center\"> Это автоматически сгенерированный emai, пожалуйста, не отвечайте на него. Все права защищены. © 2024 Redi </p> </td> </tr> <tr style=\"padding:0;vertical-align:top;text-align:left;height:50px\" valign=\"top\" align=\"left\"> <td style=\"vertical-align:top;word-wrap:break-word;border-collapse:collapse;padding:0;text-align:left;color:#222222;font-family:Lato,Arial,sans-serif;font-weight:normal;margin:0;line-height:19px;font-size:14px\" valign=\"top\" align=\"left\">&nbsp;</td> </tr> </tbody> </table> </center> </td> </tr> </tbody></table>";

            using MailMessage msg = new()
            {
                From = new MailAddress(_sMTPConfiguration.Email),
                Subject = "Востановление пароля",
                IsBodyHtml = true,
                Body = string.Format(mailBody)
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

        private AlternateView GetEmbeddedImage(string filePath)
        {
            LinkedResource res = new(filePath)
            {
                ContentId = Guid.NewGuid().ToString()
            };
            string htmlBody = @"<img src='cid:" + res.ContentId + @"'/>";
            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(res);
            return alternateView;
        }
    }
}

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using Wheel.DependencyInjection;

namespace Wheel.Email
{
    public class MailKitSender(IOptions<MailKitOptions> mailKitOptions) : IEmailSender, ITransientDependency
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();
            //发件人
            message.From.Add(new MailboxAddress(mailKitOptions.Value.SenderName, mailKitOptions.Value.UserName));
            //收件人
            message.To.Add(new MailboxAddress(subject, email));

            //标题
            message.Subject = subject;
            //正文内容，发送
            message.Body = new BodyBuilder
            {
                HtmlBody = htmlMessage
            }.ToMessageBody();

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                //Smtp服务器
                client.Connect(mailKitOptions.Value.Host, mailKitOptions.Value.Prot, mailKitOptions.Value.UseSsl);
                //登录，发送
                client.Authenticate(mailKitOptions.Value.UserName, mailKitOptions.Value.Password);
                await client.SendAsync(message);
                //断开
                client.Disconnect(true);
            }
        }
    }
}

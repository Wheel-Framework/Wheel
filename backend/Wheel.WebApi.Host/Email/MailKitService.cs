using DotNetCore.CAP.Messages;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Wheel.DependencyInjection;

namespace Wheel.Email
{
    public class MailKitService : IEmailSender, ITransientDependency
    {
        private readonly IOptions<MailKitOptions> _mailKitOptions;

        public MailKitService(IOptions<MailKitOptions> mailKitOptions)
        {
            _mailKitOptions = mailKitOptions;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();
            //发件人
            message.From.Add(new MailboxAddress(_mailKitOptions.Value.SenderName, _mailKitOptions.Value.UserName));
            //收件人
            message.To.Add(new MailboxAddress(subject, email));
            //   message.To.Add(new MailboxAddress(title,mailName ));

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
                client.Connect(_mailKitOptions.Value.Host, _mailKitOptions.Value.Prot, _mailKitOptions.Value.UseSsl);
                //登录，发送
                client.Authenticate(_mailKitOptions.Value.UserName, _mailKitOptions.Value.Password);
                await client.SendAsync(message);
                //断开
                client.Disconnect(true);
            }
        }
    }
}

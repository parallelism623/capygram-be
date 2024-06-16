using capygram.Common.DTOs.User;
using capygram.Notification.DependencyInjection.Options;

using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;


namespace capygram.Notification.Services
{
    public class SendMailServices : ISendMailServices
    {
        private readonly SendMailOptions _sendMailOptions;
        public SendMailServices(IOptionsMonitor<SendMailOptions> sendMailOptions)
        {
            _sendMailOptions = sendMailOptions.CurrentValue;
        }

        public async Task SendMailHtml(SendMailHtml mail)
        {

            using (MimeMessage emailMessage = new MimeMessage())
            {
                MailboxAddress emailFrom = new MailboxAddress(_sendMailOptions.UserName, _sendMailOptions.From);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(mail.UsernameReceive, mail.EmailReceive);
                emailMessage.To.Add(emailTo);

                //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com")); 
                //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                emailMessage.Subject = mail.Title;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = mail.Body;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();
                //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_sendMailOptions.SmtpServer, _sendMailOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_sendMailOptions.From, _sendMailOptions.Password);
                    await mailClient.SendAsync(emailMessage);
                    mailClient.Disconnect(true);
                }
            }



        }
    }
}

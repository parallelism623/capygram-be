using capygram.Auth.MessageBus.Events;
using capygram.Common.DTOs.User;
using capygram.Notification.Services;
using MediatR;

namespace capygram.Notification.UseCases.Events
{
    public class EmailNotificationConsumerHandler : IRequestHandler<EmailNotification>
    {
        private readonly ISendMailServices _sendMail;
        public EmailNotificationConsumerHandler(ISendMailServices sendMail)
        {
            _sendMail = sendMail;
        }
        public async Task Handle(EmailNotification request, CancellationToken cancellationToken)
        {
            var sendMailHtml = new SendMailHtml();
            sendMailHtml.Title = string.Format("Hello {0}", request.nameTo);
            sendMailHtml.EmailReceive = request.mailTo;
            sendMailHtml.Body = request.content;
            sendMailHtml.UsernameReceive = request.Name;
            await _sendMail.SendMailHtml(sendMailHtml);
        }
    }
}

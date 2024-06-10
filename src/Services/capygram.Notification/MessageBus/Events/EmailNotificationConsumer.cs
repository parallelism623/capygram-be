using capygram.Auth.MessageBus.Events;
using capygram.Common.Abstraction;
using MediatR;

namespace capygram.Notification.MessageBus.Events
{
    public class EmailNotificationConsumer : Consumer<EmailNotification>
    {
        public EmailNotificationConsumer(ISender sender) : base(sender)
        {
        }
        
    }
}

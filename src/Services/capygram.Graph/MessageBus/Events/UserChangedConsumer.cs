using capygram.Common.Abstraction;
using capygram.Common.MessageBus.Events;
using MediatR;

namespace capygram.Graph.MessageBus.Events
{
    public class UserChangedConsumer : Consumer<UserChangedNotification>
    {
        public UserChangedConsumer(ISender sender) : base(sender)
        {
        }

    }
}

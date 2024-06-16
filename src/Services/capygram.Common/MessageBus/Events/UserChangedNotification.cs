using capygram.Common.Abstraction;
using capygram.Common.DTOs.User;

namespace capygram.Common.MessageBus.Events
{
    public class UserChangedNotification : INotification
    {
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Type { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;
        public UserChangedNotificationDto User { get; set; } = new UserChangedNotificationDto();
    }
}

using capygram.Common.Abstraction;
using capygram.Common.DTOs.User;

namespace capygram.Auth.MessageBus.Events
{
    public class EmailNotification : INotification
    {
        public string content { get; set; }
        public string mailTo { get; set; }
        public string nameTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; } = DateTimeOffset.UtcNow;

    }
}

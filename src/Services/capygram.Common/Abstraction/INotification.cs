using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace capygram.Common.Abstraction
{
    [ExcludeFromTopology]
    public interface INotification : IMessage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}

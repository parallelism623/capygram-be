using MassTransit;
using MediatR;


namespace capygram.Common.Abstraction
{
    public abstract class Consumer<TMessage> : IConsumer<TMessage>
        where TMessage : class, INotification
    {
        private readonly ISender _sender;
        public Consumer(ISender sender)
        {
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<TMessage> context)
            => await _sender.Send(context.Message);
    }
}

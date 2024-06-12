using capygram.Graph.MessageBus.Events;
using capygram.Graph.Repositories;
using MediatR;

namespace capygram.Graph.UseCases.Events
{
    public class UserChangedConsumerHandler : IRequestHandler<UserChangedConsumer>
    {
        private readonly IPersonRepository _personRepository;
        public UserChangedConsumerHandler(IPersonRepository personRepository)
        {
            _personRepository = personRepository;   
        }
        public Task Handle(UserChangedConsumer request, CancellationToken cancellationToken)
        {
            
        }

    }
}

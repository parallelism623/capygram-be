using capygram.Common.DTOs.User;
using capygram.Common.MessageBus.Events;
using capygram.Graph.MessageBus.Events;
using capygram.Graph.Repositories;
using capygram.Graph.Services;
using MediatR;

namespace capygram.Graph.UseCases.Events
{
    public class UserChangedConsumerHandler : IRequestHandler<UserChangedNotification>
    {
        private readonly IPersonServices _personServices;
        public UserChangedConsumerHandler(IPersonServices personServices)
        {
            _personServices = personServices;   
        }

        public async Task Handle(UserChangedNotification request, CancellationToken cancellationToken)
        {
            switch(request.Type)
            {
                case "add": await HandleAddUser(request.User); break;
                case "update": await HandleUpdateUser(request.User); break;
                case "delete": await HandleDeleteUser(request.User); break;
            }
        }
        private async Task HandleAddUser(UserChangedNotificationDto user)
        {
            await _personServices.AddAsync<UserChangedNotificationDto>(user);
        }
        private async Task HandleUpdateUser(UserChangedNotificationDto user)
        {
            await _personServices.UpdateAsync<UserChangedNotificationDto>(user);
        }
        private async Task HandleDeleteUser(UserChangedNotificationDto user)
        {
            await _personServices.RemoveAsync<UserChangedNotificationDto>(user);
        }
    }
}

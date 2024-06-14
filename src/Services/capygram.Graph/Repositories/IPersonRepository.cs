using capygram.Common.DTOs.User;

namespace capygram.Graph.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<T>> GetAllAsync<T>(Guid Id, string relationship);
        Task<bool> AddAsync(UserChangedNotificationDto entities);
        Task<bool> AddRelationshipAsync(Guid id, Guid did);
        Task<bool> DeleteRelationshipAsync(Guid id, Guid did);
        Task<bool> IsHaveRelationShip(Guid id, Guid did);
 
    }
}

using capygram.Common.Shared;

namespace capygram.Graph.Services
{
    public interface IPersonServices
    {
        Task<Result<int>> GetCountFollowAsync<T>(Guid Id, string relationship);
        Task<Result<List<T>>> GetAllFollowAsync<T>(Guid Id, string relationship);
        Task<Result<string>> AddAsync<T>(T entities);
        Task<Result<string>> AddRelationshipAsync(Guid id, Guid did);
        Task<Result<string>> UpdateAsync<T>(T entities);
        Task<Result<string>> RemoveAsync<T>(T entities);
        Task<Result<string>> DeleteRelationshipAsync(Guid id, Guid did);
    }
}

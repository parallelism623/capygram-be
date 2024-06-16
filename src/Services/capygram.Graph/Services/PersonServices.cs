using capygram.Common.DTOs.User;
using capygram.Common.Exceptions;
using capygram.Common.Shared;
using capygram.Graph.Repositories;
using Neo4jClient;

namespace capygram.Graph.Services
{
    public class PersonServices : IPersonServices
    {
        private readonly IPersonRepository _personRepository;

        public PersonServices(IPersonRepository personRepository) 
        {
            _personRepository = personRepository;
        }
        public async Task<Result<int>> GetCountFollowAsync<T>(Guid Id, string relationship)
        {
            var result = (await _personRepository.GetAllAsync<T>(Id, relationship)).Count();
            return result;
        }
        public async Task<Result<List<T>>> GetAllFollowAsync<T>(Guid Id, string relationship)
        {
            var result = (await _personRepository.GetAllAsync<T>(Id, relationship)).ToList();
            return result;
        }
        public async Task<Result<string>> AddAsync<T>(T entities)
        {
            var result = await _personRepository.AddAsync(entities as UserChangedNotificationDto);
            return "Add New User Success";
        }
        public async Task<Result<string>> AddRelationshipAsync(Guid id, Guid did)
        {
            if (await _personRepository.IsHaveRelationShip(id, did))
            {
                throw new BadRequestException("Has Relationship Between Two Users");
            }
            await _personRepository.AddRelationshipAsync(id, did);
            return "Add New Relationship Success";
        }
        public async Task<Result<string>> DeleteRelationshipAsync(Guid id, Guid did)
        {
            if (await _personRepository.IsHaveRelationShip(id, did))
            {
                throw new BadRequestException("No Has Relationship Between Two Users");
            }
            await _personRepository.DeleteRelationshipAsync(id, did);
            return "Delete Relationship Success";
        }

        public async Task<Result<string>> UpdateAsync<T>(T entities)
        {
            var result = await _personRepository.AddAsync(entities as UserChangedNotificationDto);
            return "Update User Success";
        }

        public async Task<Result<string>> RemoveAsync<T>(T entities)
        {
            var result = await _personRepository.RemoveAsync(entities as UserChangedNotificationDto);
            return "Remove User Success";
        }
    }
}

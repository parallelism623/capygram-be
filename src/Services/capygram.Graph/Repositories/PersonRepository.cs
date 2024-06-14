using capygram.Common.DTOs.User;
using capygram.Graph.DependencyInjection.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neo4j.Driver;
using Neo4jClient;
using Newtonsoft.Json.Serialization;

namespace capygram.Graph.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IGraphClient _client;

        public PersonRepository(IGraphClient client)
        {

            _client = client;

        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(Guid Id, string relationship)
        {

            var result = await _client.Cypher
                .Match($"(A) -[:{relationship}]->(B)")
                .Where((UserChangedNotificationDto A) => A.Id == Id)
                .Return((B) =>
                new
                {
                    RelationshipPerson = B.As<T>()
                })
                .ResultsAsync;
            var finalResult = result.Select(x => x.RelationshipPerson).ToList();
            return finalResult;
        }
        public async Task<bool> AddAsync(UserChangedNotificationDto entities)
        {
           
            try
            {
                await _client.Cypher
                    .Merge("(user{ Id: {id} })")
                    .OnCreate()
                    .Set("user = {newUser}")
                    .WithParams(new
                    {
                        id = entities.Id,
                        entities
                    })
                    .ExecuteWithoutResultsAsync();
                    return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }


        public async Task<bool> AddRelationshipAsync(Guid id, Guid did)
        {
            await _client.Cypher.Match("(start)", "(end)")
                .Where((UserChangedNotificationDto start) => start.Id == id)
                .AndWhere((UserChangedNotificationDto end) => end.Id == did)
                .Create("(start)-[:FOLLOWING]->(end)").ExecuteWithoutResultsAsync();
            await _client.Cypher.Match("(start)", "(end)")
                .Where((UserChangedNotificationDto start) => start.Id == id)
                .AndWhere((UserChangedNotificationDto end) => end.Id == did)
                .Create("(start)<-[:FOLLOWER]-(end)").ExecuteWithoutResultsAsync();
            return true;
        }
        public async Task<bool> DeleteRelationshipAsync(Guid id, Guid did)
        {
           
            await _client.Cypher.Match("(start)-[r:FOLLOWING]->(end)")
                .Where((UserChangedNotificationDto start) => start.Id == id)
                .AndWhere((UserChangedNotificationDto end) => end.Id == did)
                .Delete("r").ExecuteWithoutResultsAsync();
            await _client.Cypher.Match("(start)<-[r:FOLLOWER]-(end)")
                .Where((UserChangedNotificationDto start) => start.Id == id)
                .AndWhere((UserChangedNotificationDto end) => end.Id == did)
                .Delete("r").ExecuteWithoutResultsAsync();
            return true;
        }
        public async Task<bool> IsHaveRelationShip(Guid id, Guid did)
        {
            var result = await _client.Cypher.Match("(start)-[r:FOLLOWING]->(end)")
                .Where((UserChangedNotificationDto start) => start.Id == id)
                .AndWhere((UserChangedNotificationDto end) => end.Id == did)
                .Return(r => new
                {
                    Node = r.As<RelationshipInstance>()
                }).ResultsAsync;
            return result.Any();
        }


    }


    
}

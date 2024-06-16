using capygram.Common.DTOs.User;
using capygram.Graph.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neo4jClient;
using System;

namespace capygram.Graph.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonServices _personServices;

        public PersonsController(IPersonServices personServices)
        {
            _personServices = personServices;
        }

        [HttpGet("{Id}/follower")]
        public async Task<IActionResult> GetFollowerAsync(Guid Id)
        {
            var result = await _personServices.GetAllFollowAsync<UserChangedNotificationDto>(Id, "FOLLOWER");
            return Ok(result);
        }
        [HttpGet("{Id}/following")]
        public async Task<IActionResult> GetFollowingAsync(Guid Id)
        {
            var result = await _personServices.GetAllFollowAsync<UserChangedNotificationDto>(Id, "FOLLOWING");
            return Ok(result);
        }
        [HttpPost("{id}/follow/{did}")]
        public async Task<IActionResult> AddFollowAsync(Guid id, Guid did)
        {
            var result = await _personServices.AddRelationshipAsync(id, did);
            return Ok(result);
        }
        [HttpPost("{id}/unfollow/{did}")]
        public async Task<IActionResult> UnFollowAsync(Guid id, Guid did)
        {
            var result = await _personServices.DeleteRelationshipAsync(id, did);
            return Ok(result);
        }
        [HttpGet("{Id}/following/count")]
        public async Task<IActionResult> GetFollowingCountAsync(Guid Id)
        {
            var result = await _personServices.GetCountFollowAsync<UserChangedNotificationDto>(Id, "FOLLOWING");
            return Ok(result);
        }
        [HttpGet("{Id}/follower/count")]
        public async Task<IActionResult> GetFollowerCountAsync(Guid Id)
        {
            var result = await _personServices.GetCountFollowAsync<UserChangedNotificationDto>(Id, "FOLLOWER");
            return Ok(result);
        }
    }
}

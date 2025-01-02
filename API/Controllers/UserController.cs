using API.DTOs;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IUnitOfWork uow) : BaseAPIController
    {
        [HttpGet("{username}/")]
        public async Task<ActionResult<MemberDTO?>> GetUserByUsername(string username)
        {
            return await uow.UserRepository.GetMemberByUsernameAsync(username);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await uow.UserRepository.GetMembersAsync();
            return Ok(users);
        }
    }
}
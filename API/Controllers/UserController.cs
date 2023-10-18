using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public UserController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("{username}/")]
        public async Task<ActionResult<MemberDTO>> GetUserByUsername(string username)
        {
            return await _uow.UserRepository.GetMemberByUsernameAsync(username);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await _uow.UserRepository.GetMembersAsync();
            return Ok(users);
        }
    }
}
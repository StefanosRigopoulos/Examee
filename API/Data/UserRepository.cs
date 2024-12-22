using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public void Update(AppUser user) {
            _context.Entry(user).State = EntityState.Modified;
        }
        // AppUser
        public async Task<AppUser> GetUserByIdAsync(int id) {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username) {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<AppUser> GetUserByEmailAsync(string email) {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync() {
            return await _context.Users.ToListAsync();
        }

        // MemberDTO
        public async Task<MemberDTO> GetMemberByUsernameAsync(string username) {
            return await _context.Users.Where(x => x.UserName == username).Include(x => x.Exams).ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<MemberDTO>> GetMembersAsync() {
            return await _context.Users.ProjectTo<MemberDTO>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
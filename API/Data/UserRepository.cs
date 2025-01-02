using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
    {
        public void Update(AppUser user)
        {
            context.Entry(user).State = EntityState.Modified;
        }
        public void Delete(AppUser user)
        {
            context.Remove(user);
        }

        // AppUser
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await context.Users.FindAsync(id);
        }
        public async Task<AppUser?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await context.Users.SingleOrDefaultAsync(x => x.Email == email);
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        // MemberDTO
        public async Task<MemberDTO?> GetMemberByUsernameAsync(string username)
        {
            return await context.Users.Where(x => x.UserName == username).Include(x => x.Exams).ProjectTo<MemberDTO>(mapper.ConfigurationProvider).SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<MemberDTO>> GetMembersAsync()
        {
            return await context.Users.ProjectTo<MemberDTO>(mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        
        Task<AppUser> GetUserByIdAsync(int id);
        Task<AppUser> GetUserByUsernameAsync(string username);
        Task<AppUser> GetUserByEmailAsync(string email);
        Task<IEnumerable<AppUser>> GetUsersAsync();

        Task<MemberDTO> GetMemberByUsernameAsync(string username);
        Task<IEnumerable<MemberDTO>> GetMembersAsync();
    }
}
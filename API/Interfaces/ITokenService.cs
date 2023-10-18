using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<String> CreateToken(AppUser user);
    }
}
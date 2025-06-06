using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseAPIController
    {
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO) {
            if (await UsernameExists(registerDTO.UserName)) return BadRequest(new ApiException(400, "Information duplicate", "User already present with this name"));
            if (await EmailExists(registerDTO.Email)) return BadRequest(new ApiException(400, "Information duplicate", "Email is already taken"));

            var user = mapper.Map<AppUser>(registerDTO);

            user.UserName = registerDTO.UserName.ToLower();

            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            var roleResult = await userManager.AddToRoleAsync(user, registerDTO.Role);
            if (!roleResult.Succeeded) return BadRequest(result.Errors);

            return new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await tokenService.CreateToken(user),
                PhotoURL = user.PhotoURL,
                Role = user.Role
            };
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO) {

            // Checking the username
            var user = await userManager.Users.SingleOrDefaultAsync(x => x.Email == loginDTO.Email);
            if (user == null) return Unauthorized(new ApiException(401, "Invalid Email!", "Please try again!"));

            // Checking the password
            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) return Unauthorized(new ApiException(401, "Invalid Password!", "Please try again!"));

            return new UserDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = await tokenService.CreateToken(user),
                PhotoURL = user.PhotoURL,
                Role = user.Role
            };
        }

        private async Task<bool> UsernameExists(string username){
            return await userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

        private async Task<bool> EmailExists(string email){
            return await userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }
    }
}
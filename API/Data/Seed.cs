using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole{ Name = "Student" },
                new AppRole{ Name = "Professor" },
                new AppRole{ Name = "Admin" }
            };

            foreach(var role in roles) {
                await roleManager.CreateAsync(role);
            }
            foreach (var user in users!) {
                user.UserName = user.UserName!.ToLower();
                user.Created = DateTime.SpecifyKind(user.Created, DateTimeKind.Utc);
                await userManager.CreateAsync(user, "Password1");
            }
        }
    }
}
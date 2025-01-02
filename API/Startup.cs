using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Startup(IConfiguration config)
    {
        // This method gets called by the runtime. Use this method to add services to the container. (Dependency Injection Container)
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationServices(config);
            services.AddIdentityServices(config);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:4200"));

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });

            // Temp seed data
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            try {
                var context = services.GetRequiredService<DataContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();
                var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
                await context.Database.MigrateAsync();
                await Seed.SeedUsers(userManager, roleManager);
            } catch (Exception error) {
                var logger = services.GetService<ILogger<AppUser>>();
                logger!.LogError(error, "An error occurred during migration");
            }
        }
    }
}
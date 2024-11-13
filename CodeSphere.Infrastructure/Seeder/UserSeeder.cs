using CodeSphere.Domain.Models.Identity;
using CodeSphere.Domain.Premitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CodeSphere.Infrastructure.Seeder
{
    public static class UserSeeder
    {

        public async static Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.Users.CountAsync() == 0)
            {

                var defaultUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@project.com",
                    PhoneNumber = "123456",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(defaultUser, "Aa@123.123");
                await userManager.AddToRoleAsync(defaultUser, Roles.Admin);
            }
        }
    }
}

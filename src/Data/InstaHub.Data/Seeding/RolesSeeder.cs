namespace InstaHub.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Common;
    using InstaHub.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var passwordHasher = serviceProvider.GetRequiredService<IPasswordHasher<ApplicationUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);

            var userAdminId = await SeedUserAdmin(dbContext, passwordHasher);

            if (userAdminId != null)
            {
                await SeedUserToRoleAsync(userManager, GlobalConstants.AdministratorRoleName, userAdminId);
            }
        }

        private static async Task<string> SeedUserAdmin(ApplicationDbContext dbContext, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            if (await dbContext.Users.AnyAsync(x => x.UserName == "admin"))
            {
                return null;
            }

            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                ImagePath = "adminNinja.jpg",
            };

            var pass = passwordHasher.HashPassword(user, "123456");
            user.PasswordHash = pass;

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            return user.Id;
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static async Task SeedUserToRoleAsync(UserManager<ApplicationUser> userManager, string roleName, string userAdminId)
        {
            var user = await userManager.FindByIdAsync(userAdminId);

            if (user == null)
            {
                return;
            }

            var isInRole = await userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                return;
            }

            await userManager.AddToRoleAsync(user, roleName);
        }
    }
}

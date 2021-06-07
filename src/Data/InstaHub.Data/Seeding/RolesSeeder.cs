namespace InstaHub.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using InstaHub.Common;
    using InstaHub.Data.Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);

            await SeedUserToRoleAsync(userManager, GlobalConstants.AdministratorRoleName);
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

        private static async Task SeedUserToRoleAsync(UserManager<ApplicationUser> userManager, string roleName)
        {
            var user = await userManager.FindByIdAsync("d84e64d5-87cc-47e7-ab60-b40f388e91a4");

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

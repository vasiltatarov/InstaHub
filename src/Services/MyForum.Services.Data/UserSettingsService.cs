namespace MyForum.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;

        public UserSettingsService(IRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task AddDescription(string userId, string description)
        {
            var user = await this.usersRepository.All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return;
            }

            if (user.Description == description)
            {
                return;
            }

            user.Description = description;
            await this.usersRepository.SaveChangesAsync();
        }
    }
}

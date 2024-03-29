﻿namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class UserSettingsService : IUserSettingsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;

        public UserSettingsService(IRepository<ApplicationUser> usersRepository)
            => this.usersRepository = usersRepository;

        public async Task AddDescription(string userId, string description)
        {
            var user = await this.usersRepository.All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null || user.Description == description)
            {
                return;
            }

            user.Description = description;
            await this.usersRepository.SaveChangesAsync();
        }
    }
}

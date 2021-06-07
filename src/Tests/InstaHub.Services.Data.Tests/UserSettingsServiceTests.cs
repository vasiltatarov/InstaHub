namespace InstaHub.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using InstaHub.Data;
    using InstaHub.Data.Models;
    using InstaHub.Data.Repositories;
    using Xunit;

    public class UserSettingsServiceTests
    {
        private readonly EfRepository<ApplicationUser> repo;
        private readonly UserSettingsService service;
        private readonly ApplicationDbContext db;

        public UserSettingsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            this.db = new ApplicationDbContext(options);
            this.repo = new EfRepository<ApplicationUser>(db);
            this.service = new UserSettingsService(this.repo);
        }

        [Theory]
        [InlineData("v12", "My name is Vasq")]
        public async Task AddDescriptionShouldWorkCorrect(string userId, string description)
        {
            await this.repo.AddAsync(new ApplicationUser
            {
                Id = userId,
                Email = "vasko@gmail.com",
                UserName = "vasko",
            });
            await this.repo.SaveChangesAsync();

            await this.service.AddDescription(userId, description);

            var user = await this.repo.AllAsNoTracking().FirstOrDefaultAsync();

            Assert.Equal(user.Description, description);
        }

        [Theory]
        [InlineData("v12", "My name is Vasq")]
        public async Task AddDescriptionShouldDoesNotWorkWithUnexistingUser(string userId, string description)
        {
            await this.service.AddDescription(userId, description);

            var user = await this.repo.AllAsNoTracking().FirstOrDefaultAsync();

            Assert.Null(user);
        }

        [Theory]
        [InlineData("v12", "My name is Vasq")]
        public async Task AddDescriptionShouldDoesNotOverrideDescription(string userId, string description)
        {
            await this.repo.AddAsync(new ApplicationUser
            {
                Id = userId,
                Email = "vasko@gmail.com",
                UserName = "vasko",
                Description = description,
            });
            await this.repo.SaveChangesAsync();

            await this.service.AddDescription(userId, description);

            var user = await this.repo.AllAsNoTracking().FirstOrDefaultAsync();

            Assert.Equal(user.Description, description);
        }
    }
}

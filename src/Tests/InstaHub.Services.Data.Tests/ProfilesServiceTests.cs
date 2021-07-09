namespace InstaHub.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using InstaHub.Data;
    using InstaHub.Data.Models;
    using InstaHub.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ProfilesServiceTests
    {
        private readonly EfDeletableEntityRepository<ApplicationUser> userRepo;
        private readonly ProfileService service;

        public ProfilesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.userRepo = new EfDeletableEntityRepository<ApplicationUser>(db);

            var userFollow = new EfRepository<UserFollow>(db);
            var followRepo = new FollowService(userFollow, this.userRepo);

            this.service = new ProfileService(this.userRepo, followRepo);

            var map = new MapperInitializationProfile();
        }

        [Theory]
        [InlineData("vasko", "v1", "c2", "img.jpg")]
        public async Task GetUserPostsShouldReturnPostsCorrectly(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            await this.userRepo.AddAsync(new ApplicationUser
            {
                UserName = username,
                Id = currentUserId,
                Email = username + "@abv.bg",
                PasswordHash = username + "123",
                ImagePath = currentUserImagePath,
                Description = "adssdaasd",
            });

            var res = await this.service.GetUserPosts(username, currentUserId, followedUserId, currentUserImagePath);

            ;
        }
    }
}

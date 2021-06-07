namespace InstaHub.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using InstaHub.Data;
    using InstaHub.Data.Models;
    using InstaHub.Data.Repositories;
    using InstaHub.Services.Data.Tests.Models;
    using Xunit;

    public class FollowsServiceTests
    {
        private readonly EfRepository<UserFollow> repo;
        private readonly FollowService service;

        public FollowsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);

            this.repo = new EfRepository<UserFollow>(db);
            var userRepo = new EfRepository<ApplicationUser>(db);

            this.service = new FollowService(this.repo, userRepo);

            _ = new MapperInitializationProfile();
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task FollowAsyncShouldWorkCorrect(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);

            var all = this.repo.All();
            var follow = await this.repo.All().FirstOrDefaultAsync();

            Assert.Single(all);
            Assert.Equal(followerId, follow.FollowerId);
            Assert.Equal(followedId, follow.FollowedId);
            Assert.True(follow.IsFollowActive);
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task FollowAsyncShouldReturnWhenFollowIsExist(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);
            await this.service.FollowAsync(followerId, followedId);

            var all = this.repo.All();
            var follow = await this.repo.All().FirstOrDefaultAsync();

            Assert.Single(all);
            Assert.Equal(followerId, follow.FollowerId);
            Assert.Equal(followedId, follow.FollowedId);
            Assert.True(follow.IsFollowActive);
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task FollowAsyncShouldChangeIsFollowActiveOnTrue(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);
            await this.service.UnfollowAsync(followerId, followedId);
            await this.service.FollowAsync(followerId, followedId);

            var all = this.repo.All();
            var follow = await this.repo.All().FirstOrDefaultAsync();

            Assert.Single(all);
            Assert.Equal(followerId, follow.FollowerId);
            Assert.Equal(followedId, follow.FollowedId);
            Assert.True(follow.IsFollowActive);
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task UnfollowAsyncShouldWorkCorrect(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);
            await this.service.UnfollowAsync(followerId, followedId);

            var all = this.repo.All();
            var follow = await this.repo.All().FirstOrDefaultAsync();

            Assert.Single(all);
            Assert.Equal(followerId, follow.FollowerId);
            Assert.Equal(followedId, follow.FollowedId);
            Assert.False(follow.IsFollowActive);
        }

        [Theory]
        [InlineData("vs1")]
        public async Task GetFollowersByUserIdShouldWorkCorrect(string userId)
        {
            await this.service.FollowAsync("ss1", userId);
            await this.service.FollowAsync("sv2", userId);
            await this.service.FollowAsync("sv3", userId);
            await this.service.FollowAsync("sv3", "sss");

            await this.service.FollowAsync("aa1", userId);
            await this.service.UnfollowAsync("aa1", userId);

            var follows = this.service.GetFollowersByUserId<FollowModel>(userId);

            Assert.Equal(3, follows.Count());
        }

        [Theory]
        [InlineData("vs1")]
        public void GetFollowersByUserIdShouldReturnEmptyCollection(string userId)
        {
            var follows = this.service.GetFollowersByUserId<FollowModel>(userId);

            Assert.Empty(follows);
        }

        [Theory]
        [InlineData("vs1")]
        public async Task GetFollowedByUserIdShouldWorkCorrect(string userId)
        {
            await this.service.FollowAsync(userId, "ss1");
            await this.service.FollowAsync(userId, "sv2");
            await this.service.FollowAsync(userId, "sv3");
            await this.service.FollowAsync("sv3", "sss");

            await this.service.FollowAsync(userId, "vvv");
            await this.service.UnfollowAsync(userId, "vvv");

            var follows = this.service.GetFollowedByUserId<FollowModel>(userId);

            Assert.Equal(3, follows.Count());
        }

        [Theory]
        [InlineData("vs1")]
        public void GetFollowedByUserIdShouldReturnEmptyCollection(string userId)
        {
            var follows = this.service.GetFollowedByUserId<FollowModel>(userId);

            Assert.Empty(follows);
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task CheckIfFollowExistAsyncShouldReturnTrueIfFollowExist(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);

            var isFollowExist = await this.service.CheckIfFollowExistAsync(followerId, followedId);

            Assert.True(isFollowExist);
        }

        [Theory]
        [InlineData("v12", "s11")]
        public async Task CheckIfFollowExistAsyncShouldReturnTrueIfFollowDoesNotExist(string followerId, string followedId)
        {
            await this.service.FollowAsync(followerId, followedId);
            await this.service.UnfollowAsync(followerId, followedId);

            var isFollowExist = await this.service.CheckIfFollowExistAsync(followerId, followedId);

            Assert.False(isFollowExist);
        }
    }
}

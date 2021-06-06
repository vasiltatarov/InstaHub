namespace MyForum.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MyForum.Data;
    using MyForum.Data.Models;
    using MyForum.Data.Repositories;
    using MyForum.Services.Data.Tests.Models;
    using Xunit;

    public class UserSavedPostsServiceTests
    {
        private readonly EfDeletableEntityRepository<UserSavedPost> repo;
        private readonly UserSavedPostsService service;

        public UserSavedPostsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.repo = new EfDeletableEntityRepository<UserSavedPost>(db);
            this.service = new UserSavedPostsService(this.repo);

            _ = new MapperInitializationProfile();
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task AddAsyncShouldWorkCorrect(string userId, int postId)
        {
            var result = await this.service.AddAsync(userId, postId);

            Assert.True(result);
            Assert.Single(this.repo.All());
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task AddAsyncShouldReturnFalseAndNotAddAgain(string userId, int postId)
        {
            var firstAddResult = await this.service.AddAsync(userId, postId);
            var secondAddResult = await this.service.AddAsync(userId, postId);

            Assert.True(firstAddResult);
            Assert.False(secondAddResult);
            Assert.Single(this.repo.All());
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task AddAsyncShouldChangeIsDeletedOnFalse(string userId, int postId)
        {
            var firstAddResult = await this.service.AddAsync(userId, postId);

            var savePost = await this.repo.All().FirstOrDefaultAsync();
            savePost.IsDeleted = true;
            await this.repo.SaveChangesAsync();

            var secondAddResult = await this.service.AddAsync(userId, postId);

            Assert.True(firstAddResult);
            Assert.True(secondAddResult);
            Assert.Single(this.repo.All());
        }

        [Theory]
        [InlineData("v12")]
        public async Task GetUserSavedPostsShouldWorkCorrect(string userId)
        {
            await this.service.AddAsync(userId, 1);
            await this.service.AddAsync(userId, 2);
            await this.service.AddAsync(userId, 3);
            await this.service.AddAsync("s22", 4);

            var actual = this.service.GetUserSavedPosts<UserSavePostModel>(userId);

            Assert.Equal(3, actual.Count());
        }

        [Theory]
        [InlineData("v12")]
        public async Task GetUserSavedPostsIfUserSaveSamePostAgainQuantityMustBeTheSame(string userId)
        {
            await this.service.AddAsync(userId, 1);
            await this.service.AddAsync(userId, 1);

            var actual = this.service.GetUserSavedPosts<UserSavePostModel>(userId);

            Assert.Single(actual);
        }

        [Theory]
        [InlineData("v12")]
        public void GetUserSavedPostsShouldReturnEmptyCollection(string userId)
        {
            var actual = this.service.GetUserSavedPosts<UserSavePostModel>(userId);

            Assert.Empty(actual);
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task DeleteShouldWorkCorrect(string userId, int postId)
        {
            await this.service.AddAsync(userId, postId);
            await this.service.Delete(userId, postId);

            var savePostWithoutDeleted = await this.repo.All().FirstOrDefaultAsync();
            var savePost = await this.repo.AllWithDeleted().FirstOrDefaultAsync();

            Assert.Null(savePostWithoutDeleted);
            Assert.True(savePost.IsDeleted);
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task DeleteShouldNotDeleteSavePost(string userId, int postId)
        {
            await this.service.Delete(userId, postId);

            var savePostWithoutDeleted = await this.repo.All().FirstOrDefaultAsync();
            var savePost = await this.repo.AllWithDeleted().FirstOrDefaultAsync();

            Assert.Null(savePostWithoutDeleted);
            Assert.Null(savePost);
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task IsPostSavedShouldWorkCorrect(string userId, int postId)
        {
            await this.service.AddAsync(userId, postId);

            var result = await this.service.IsPostSaved(userId, postId);

            Assert.True(result);
        }

        [Theory]
        [InlineData("v12", 1)]
        public async Task IsPostSavedShouldReturnfalseIfEntityNotExist(string userId, int postId)
        {
            var result = await this.service.IsPostSaved(userId, postId);

            Assert.False(result);
        }
    }
}

namespace MyForum.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MyForum.Data;
    using MyForum.Data.Models;
    using MyForum.Data.Repositories;
    using Xunit;

    public class CommentsServiceTests
    {
        private readonly EfDeletableEntityRepository<Comment> repo;
        private readonly CommentService service;

        public CommentsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.repo = new EfDeletableEntityRepository<Comment>(db);
            this.service = new CommentService(this.repo);
        }

        [Fact]
        public async Task CreateCommentShouldWorkCorrect()
        {
            await this.service.CreateComment(1, "v12", "Hi");

            Assert.Single(this.repo.All());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 3)]
        public async Task IsInPostIdShouldWorkCorrect(int commentId, int postId)
        {
            await this.service.CreateComment(postId, "v12", "Hi");
            await this.service.CreateComment(2, "v32", "Hi");

            var isInPostId = this.service.IsInPostId(commentId, postId);

            Assert.True(isInPostId);
        }

        [Theory]
        [InlineData(1, 3)]
        public async Task IsInPostIdShouldReturnFalseWhenCommentIsNotInPost(int commentId, int postId)
        {
            await this.service.CreateComment(2, "v32", "Hi");

            var isInPostId = this.service.IsInPostId(commentId, postId);

            Assert.False(isInPostId);
        }
    }
}

using MyForum.Web.ViewModels.Posts;

namespace MyForum.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using Xunit;

    public class PostServiceTests
    {
        [Fact]
        public async Task CreateAsyncShouldCreatedNewPostSuccessfully()
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostsService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");
            await service.CreateAsync("BMW", "The best BMW", 2, "v1");
            await service.CreateAsync("XUnit", "The best XUnit", 3, "v1");

            // Assert
            Assert.Equal(3, list.Count);
        }

        [Fact]
        public async Task CreateAsyncShouldCreateNewPostSuccessfullyAndReturnPostId()
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostsService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");

            // Assert
            Assert.Single(list);
            Assert.Equal(postId, list.FirstOrDefault().Id);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectPost() // Not work
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostsService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");
            var post = await service.GetById<It.IsAnyType>(postId);

            // Assert
            Assert.Single(list);
        }
    }
}

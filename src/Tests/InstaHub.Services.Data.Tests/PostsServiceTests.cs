namespace InstaHub.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;
    using InstaHub.Services.Data.Tests.Models;
    using Moq;
    using Xunit;

    public class PostsServiceTests
    {
        public PostsServiceTests()
        {
            _ = new MapperInitializationProfile();
        }

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

            var service = new PostService(mockRepo.Object);

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

            var service = new PostService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");

            // Assert
            Assert.Single(list);
            Assert.Equal(postId, list.FirstOrDefault().Id);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectPost()
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");
            var post = await service.GetById<PostModel>(postId);

            // Assert
            Assert.Equal(post.Id, postId);
            Assert.Single(list);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectPostTest()
        {
            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.AllAsNoTracking())
                .Returns(new List<Post>()
                {
                    new()
                    {
                        Id = 11,
                        Content = "vasko",
                    },
                    new()
                    {
                        Id = 12,
                        Content = "Nasko",
                    },
                }.AsQueryable());

            var service = new PostService(mockRepo.Object);

            // Act
            var post = await service.GetById<PostModel>(11);

            // Assert
            Assert.Equal(11, post.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetCountByCategoryIdShouldReturnCorrectResult(int categoryId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones", "The best", categoryId, "v1");
            await service.CreateAsync("Game of Thrones1", "The best1", categoryId, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", categoryId, "v1");
            var actual = service.GetCountByCategoryId(categoryId);

            // Assert
            Assert.Equal(3, actual);
        }

        [Theory]
        [InlineData(11)]
        public async Task GetCountByCategoryIdShouldReturnZeroWhenPostsWithGivenCategoryIdDoesNotExists(int categoryId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones", "The best", 1, "v1");
            await service.CreateAsync("Game of Thrones1", "The best1", 2, "v2");
            var actual = service.GetCountByCategoryId(categoryId);

            // Assert
            Assert.Equal(0, actual);
        }

        [Fact]
        public async Task IncreaseVisitorsCountShouldIncreasePostVisitorsCountCorrectly()
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best", 1, "v1");

            await service.IncreaseVisitorsCount(postId);
            await service.IncreaseVisitorsCount(postId);

            var postVisitorsCount = list.FirstOrDefault(x => x.Id == postId).VisitorsCount;

            // Assert
            Assert.Equal(2, postVisitorsCount);
        }

        [Theory]
        [InlineData(11)]
        public async Task IncreaseVisitorsCountShouldNotIncreaseVisitorsCountWhenPostWithGivenIdDoesNotExist(int postId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.IncreaseVisitorsCount(postId);
            var post = list.FirstOrDefault(x => x.Id == postId);

            // Assert
            Assert.Null(post);
        }

        [Fact]
        public async Task GetAllPostsShouldReturnAllPostsCorrectly()
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable);

            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", 2, "v1");
            await service.CreateAsync("Game of Thrones3", "The best4", 2, "v1");

            var posts = service.GetAllPosts<PostModel>();

            // Assert
            Assert.Equal(3, posts.Count());
        }

        [Theory]
        [InlineData(2)]
        public async Task GetByCategoryIdShouldReturnCorrectCountPosts(int categoryId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", categoryId, "v1");
            await service.CreateAsync("Game of Thrones3", "The best4", categoryId, "v3");

            var postsInCategory = service.GetByCategoryId<PostModel>(categoryId);

            // Assert
            Assert.Equal(2, postsInCategory.Count());
        }

        [Theory]
        [InlineData(22)]
        public async Task GetByCategoryIdShouldReturnEmptyCollectionIfPostsWithGivenCategoryIdNotExists(int categoryId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", 2, "v1");

            var postsInCategory = service.GetByCategoryId<PostModel>(categoryId);

            // Assert
            Assert.Empty(postsInCategory);
        }

        [Theory]
        [InlineData(2, 2)]
        public async Task GetByCategoryIdShouldReturnCorrectCountPostsWithTakeParameter(int categoryId, int take)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", categoryId, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", categoryId, "v1");
            await service.CreateAsync("Game of Thrones3", "The best3", categoryId, "v3");

            var postsInCategory = service.GetByCategoryId<PostModel>(categoryId, take);

            // Assert
            Assert.Equal(2, postsInCategory.Count());
        }

        [Theory]
        [InlineData(0)]
        public async Task EditShouldSuccessfullyEditPost(int postId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");

            await service
                .Edit(postId, "Harry Potter", "And the Half Blood Prince", 1, false, DateTime.Now, DateTime.Now, DateTime.Now);
            var post = list.FirstOrDefault();

            // Assert
            Assert.Equal("Harry Potter", post.Title);
            Assert.Equal("And the Half Blood Prince", post.Content);
        }

        [Theory]
        [InlineData(22)]
        public async Task EditShouldNotEditPostWhenIdNotExist(int postId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");

            await service
                .Edit(postId, "Harry Potter", "And the Half Blood Prince", 1, false, DateTime.Now, DateTime.Now, DateTime.Now);
            var post = list.FirstOrDefault();

            // Assert
            Assert.NotEqual("Harry Potter", post.Title);
            Assert.NotEqual("And the Half Blood Prince", post.Content);
        }

        [Theory]
        [InlineData(0)]
        public async Task DeleteShouldWorkCorrect(int postId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));
            mockRepo.Setup(x => x.Delete(It.IsAny<Post>()))
                .Callback((Post post) => post.IsDeleted = true);

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");

            var isDeleted = await service.Delete(postId);
            var post = list.FirstOrDefault();

            // Assert
            Assert.True(isDeleted);
            Assert.True(post.IsDeleted);
        }

        [Theory]
        [InlineData(22)]
        public async Task DeleteShouldNotDeletePostWhenIdNotExist(int postId)
        {
            // Arrange
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();

            mockRepo.Setup(x => x.All()).Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));
            mockRepo.Setup(x => x.Delete(It.IsAny<Post>()))
                .Callback((Post post) => post.IsDeleted = true);

            var service = new PostService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");

            var isDeleted = await service.Delete(postId);
            var post = list.FirstOrDefault();

            // Assert
            Assert.False(isDeleted);
            Assert.False(post.IsDeleted);
        }
    }
}

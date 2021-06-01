namespace MyForum.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Moq;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels;
    using MyForum.Web.ViewModels.Categories;
    using MyForum.Web.ViewModels.Posts;
    using Xunit;

    public class PostServiceTests
    {
        public PostServiceTests()
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // var config = new MapperConfiguration(cfg =>
            // {
            //    cfg.CreateMap<PostViewModel, Post>();
            // });
            // var mapper = config.CreateMapper();
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
            var list = new List<Post>();

            var mockRepo = new Mock<IDeletableEntityRepository<Post>>();
            mockRepo.Setup(x => x.AllAsNoTracking())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));

            var service = new PostsService(mockRepo.Object);

            // Act
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");
            var post = await service.GetById<PostInCategoryViewModel>(postId);

            // Assert
            Assert.Equal(post.Id, postId);
            Assert.Single(list);
        }

        [Fact]
        public async Task GetByIdShouldReturnCorrectPostTest() // For Test
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

            var service = new PostsService(mockRepo.Object);
            var postId = await service.CreateAsync("Game of Thrones", "The best ever", 1, "v1");

            // Act
            var post = await service.GetById<Post>(0);

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

            var service = new PostsService(mockRepo.Object);

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

            var service = new PostsService(mockRepo.Object);

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

            var service = new PostsService(mockRepo.Object);

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

            var service = new PostsService(mockRepo.Object);

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
                .Returns(list.AsQueryable());

            mockRepo.Setup(x => x.AddAsync(It.IsAny<Post>()))
                .Callback((Post post) => list.Add(post));
            var service = new PostsService(mockRepo.Object);

            // Act
            await service.CreateAsync("Game of Thrones1", "The best1", 1, "v2");
            await service.CreateAsync("Game of Thrones2", "The best2", 2, "v1");
            var s = mockRepo.Object.All().Count();
            var posts = service.GetAllPosts<Models>();

            // Assert
            Assert.Equal(2, posts.Count());
        }
    }

    public class Models : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }
    }
}

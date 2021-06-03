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

    public class ChatServiceTests
    {
        private readonly EfDeletableEntityRepository<ChatMessage> repo;
        private readonly ChatService service;

        public ChatServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.repo = new EfDeletableEntityRepository<ChatMessage>(db);
            this.service = new ChatService(this.repo);

            _ = new MapperInitializationProfile();
        }

        [Fact]
        public async Task CreateAsyncShouldWorkCorrect()
        {
            await this.service.CreateAsync("Hi", "v12");

            Assert.Single(this.repo.All());
        }

        [Fact]
        public async Task GetMessagesShouldWorkCorrect()
        {
            await this.service.CreateAsync("Hi", "v12");
            await this.service.CreateAsync("How are you", "v232");
            await this.service.CreateAsync("Get the fuck out from here", "s2");

            var all = this.service.GetMessages<ChatModel>();

            Assert.Equal(3, all.Count());
        }

        [Fact]
        public void GetMessagesShouldReturnEmptyCollection()
        {
            var all = this.service.GetMessages<ChatModel>();

            Assert.Empty(all);
        }
    }
}

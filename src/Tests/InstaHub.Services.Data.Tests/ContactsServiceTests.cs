namespace InstaHub.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using InstaHub.Data;
    using InstaHub.Data.Models;
    using InstaHub.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class ContactsServiceTests
    {
        private readonly EfRepository<ContactForm> repo;
        private readonly ContactService service;

        public ContactsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.repo = new EfRepository<ContactForm>(db);
            this.service = new ContactService(this.repo);
        }

        [Fact]
        public async Task Add()
        {
            await this.service.Add("vasko", "vasko@abvc.bg", "nqma", "127.1.1.0");

            Assert.Single(this.repo.All());
        }
    }
}

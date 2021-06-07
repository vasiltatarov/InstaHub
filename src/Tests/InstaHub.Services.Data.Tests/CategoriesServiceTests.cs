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

    public class CategoriesServiceTests
    {
        private readonly Category[] data;
        private readonly EfDeletableEntityRepository<Category> repo;
        private readonly CategoriesService service;

        public CategoriesServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            this.repo = new EfDeletableEntityRepository<Category>(db);
            this.service = new CategoriesService(this.repo);

            _ = new MapperInitializationProfile();

            this.data = new Category[]
            {
                new()
                {
                    Name = "Street Fitness",
                    Title = "Street Fitness",
                    Description = "Street Fitness",
                    ImageUrl = "workout.jpg",
                },
                new()
                {
                    Name = "Calisthenics",
                    Title = "Calisthenics",
                    Description = "Calisthenics",
                    ImageUrl = "workout.jpg",
                },
            };
        }

        [Fact]
        public async Task CreateAsyncShouldWorkCorrect()
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);

            Assert.Single(this.repo.All());
        }

        [Fact]
        public async Task GetAllShouldReturnAllCategoriesCorrect()
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            await this.service.CreateAsync(this.data[1].Name, this.data[1].Title, this.data[1].Description, this.data[1].ImageUrl);

            var all = this.service.GetAll<CategoryModel>();

            Assert.Equal(2, all.Count());
        }

        [Fact]
        public void GetAllShouldReturnEmptyCollection()
        {
            var all = this.service.GetAll<CategoryModel>();

            Assert.Empty(all);
        }

        [Theory]
        [InlineData(2)]
        public async Task GetAllShouldReturnAllCategoriesCorrectWhenCountIsPassed(int count)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            await this.service.CreateAsync(this.data[1].Name, this.data[1].Title, this.data[1].Description, this.data[1].ImageUrl);
            await this.service.CreateAsync(this.data[1].Name, this.data[1].Title, this.data[1].Description, this.data[1].ImageUrl);

            var all = this.service.GetAll<CategoryModel>(count);

            Assert.Equal(2, all.Count());
        }

        [Theory]
        [InlineData("Calisthenics")]
        public async Task GetByNameShouldReturnCorrectCategoryByGivenName(string name)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            await this.service.CreateAsync(this.data[1].Name, this.data[1].Title, this.data[1].Description, this.data[1].ImageUrl);

            var category = this.service.GetByName<CategoryModel>(name);

            Assert.Equal(this.data[1].Name, category.Name);
            Assert.Equal(this.data[1].ImageUrl, category.ImageUrl);
        }

        [Theory]
        [InlineData("Calisthenics")]
        public void GetByNameShouldReturnNullWhenNameNotExist(string name)
        {
            var category = this.service.GetByName<CategoryModel>(name);

            Assert.Null(category);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetByIdAsyncShouldReturnCorrectCategoryByGivenId(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            var category = await this.service.GetByIdAsync<CategoryModel>(id);

            Assert.Equal(this.data[0].Name, category.Name);
            Assert.Equal(this.data[0].ImageUrl, category.ImageUrl);
        }

        [Theory]
        [InlineData(11)]
        public async Task GetByIdAsyncShouldReturnNullWhenIdNotExist(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            var category = await this.service.GetByIdAsync<CategoryModel>(id);

            Assert.Null(category);
        }

        [Theory]
        [InlineData(1)]
        public async Task IsCategoryExistsShouldWorkCorrect(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            var isExist = await this.service.IsCategoryExists(id);

            Assert.True(isExist);
        }

        [Theory]
        [InlineData(1123)]
        public async Task IsCategoryExistsShouldReturnFalseWhenCategoryNotExist(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);
            var isExist = await this.service.IsCategoryExists(id);

            Assert.False(isExist);
        }

        [Theory]
        [InlineData(1, "Programming", "Programming", "Programming", "/images.sd")]
        public async Task UpdateShouldWorkCorrect(int id, string name, string title, string description, string imageUrl)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);

            await this.service
                .Update(id, name, title, description, imageUrl, false, DateTime.Now, DateTime.Now, DateTime.Now);
            var category = await this.service.GetByIdAsync<CategoryModel>(id);

            Assert.Equal(name, category.Name);
            Assert.Equal(imageUrl, category.ImageUrl);
        }

        [Theory]
        [InlineData(11, "Programming", "Programming", "Programming", "/images.sd")]
        public async Task UpdateShouldNotWorkCorrectWhenCategoryIsNull(int id, string name, string title, string description, string imageUrl)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);

            await this.service
                .Update(id, name, title, description, imageUrl, false, DateTime.Now, DateTime.Now, DateTime.Now);
            var category = await this.service.GetByIdAsync<CategoryModel>(id);

            Assert.Null(category);
        }

        [Theory]
        [InlineData(1)]
        public async Task DeleteShouldWorkCorrect(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);

            var isDeleted = await this.service.Delete(id);
            var category = await this.repo.AllWithDeleted().FirstOrDefaultAsync();

            Assert.True(isDeleted);
            Assert.True(category.IsDeleted);
        }

        [Theory]
        [InlineData(23)]
        public async Task DeleteShouldNotDeletePostWhenIdNotExist(int id)
        {
            await this.service.CreateAsync(this.data[0].Name, this.data[0].Title, this.data[0].Description, this.data[0].ImageUrl);

            var isDeleted = await this.service.Delete(id);
            var category = await this.repo.All().FirstOrDefaultAsync();

            Assert.False(isDeleted);
            Assert.False(category.IsDeleted);
        }
    }
}

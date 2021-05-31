namespace MyForum.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEntityRepository<Category> categoryRepository;

        public CategoriesService(IDeletableEntityRepository<Category> categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task CreateAsync(string name, string title, string description, string imageUrl)
        {
            await this.categoryRepository.AddAsync(new Category
            {
                Name = name,
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
            });
            await this.categoryRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll<T>(int? count = null)
        {
            IQueryable<Category> query = this.categoryRepository.All()
                .OrderBy(x => x.Name);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetByName<T>(string name)
        {
            var category = this.categoryRepository.All()
                .Where(x => x.Name.Replace(" ", "-") == name.Replace(" ", "-"))
                .To<T>()
                .FirstOrDefault();
            return category;
        }

        public async Task<T> GetByIdAsync<T>(int id)
            => await this.categoryRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<bool> IsCategoryExists(int id)
            => await this.categoryRepository
                .AllWithDeleted()
                .AnyAsync(x => x.Id == id);

        public async Task Update(int id, string name, string title, string description, string imageUrl, bool isDeleted, DateTime deletedOn,
            DateTime createdOn, DateTime modifiedOn)
        {
            var category = await this.categoryRepository.All().FirstOrDefaultAsync(x => x.Id == id);
            category.Name = name;
            category.Title = title;
            category.Description = description;
            category.ImageUrl = imageUrl;
            category.CreatedOn = createdOn;
            category.DeletedOn = deletedOn;
            category.ModifiedOn = modifiedOn;
            category.IsDeleted = isDeleted;

            await this.categoryRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var category = await this.categoryRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
            {
                return false;
            }

            this.categoryRepository.Delete(category);
            await this.categoryRepository.SaveChangesAsync();
            return true;
        }
    }
}

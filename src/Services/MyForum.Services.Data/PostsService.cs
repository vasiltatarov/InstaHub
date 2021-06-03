namespace MyForum.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class PostsService : IPostsService
    {
        private readonly IDeletableEntityRepository<Post> postRepository;

        public PostsService(IDeletableEntityRepository<Post> postRepository)
        {
            this.postRepository = postRepository;
        }

        /// <summary>
        /// First call IncreaseVisitorsCount(id) that will be increase visit count and after that return post.
        /// </summary>
        public async Task<T> GetById<T>(int id)
        {
            await this.IncreaseVisitorsCount(id);

            var post = this.postRepository
                .AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return post;
        }

        public async Task<int> CreateAsync(string title, string content, int categoryId, string userId)
        {
            var post = new Post
            {
                Title = title,
                Content = content,
                CategoryId = categoryId,
                UserId = userId,
            };

            await this.postRepository.AddAsync(post);
            await this.postRepository.SaveChangesAsync();

            return post.Id;
        }

        // For HomePageController.
        public IEnumerable<T> GetAllPosts<T>()
            => this.postRepository
                .AllAsNoTracking()
                .To<T>()
                .ToList();

        // For CategoriesController - Pagination
        public IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0)
        {
            var query = this.postRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.CategoryId == categoryId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetCountByCategoryId(int categoryId)
            => this.postRepository
                .All()
                .Count(x => x.CategoryId == categoryId);

        /// <summary>
        /// Increase post visitors count.
        /// </summary>
        public async Task IncreaseVisitorsCount(int id)
        {
            var post = this.postRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return;
            }

            post.VisitorsCount++;
            await this.postRepository.SaveChangesAsync();
        }

        public async Task Edit(int id, string title, string content, int categoryId, bool isDeleted, DateTime deletedOn, DateTime createdOn, DateTime modifiedOn)
        {
            var post = this.postRepository.All().FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return;
            }

            post.Title = title;
            post.Content = content;
            post.CategoryId = categoryId;
            post.CreatedOn = createdOn;
            post.DeletedOn = deletedOn;
            post.ModifiedOn = modifiedOn;
            post.IsDeleted = isDeleted;

            await this.postRepository.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var post = this.postRepository.All().FirstOrDefault(x => x.Id == id);

            if (post == null)
            {
                return false;
            }

            this.postRepository.Delete(post);
            await this.postRepository.SaveChangesAsync();

            return true;
        }
    }
}

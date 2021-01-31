namespace MyForum.Services.Data
{
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

        public T GetById<T>(int id)
            => this.postRepository.All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

        public async Task<int> CreateAsync(string title, string content, int categoryId, string userId)
        {
            var post = new Post()
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
            => this.postRepository.All()
                .OrderByDescending(x => x.CreatedOn)
                .To<T>().ToList();

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
            => this.postRepository.All().Count(x => x.CategoryId == categoryId);
    }
}

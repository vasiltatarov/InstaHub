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

        public IEnumerable<T> GetAllPosts<T>()
            => this.postRepository.All().To<T>().ToList();
    }
}

namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserSavedPostsService
    {
        Task AddAsync(string userId, int postId);

        IEnumerable<T> GetUserSavedPosts<T>(string userId);
    }
}

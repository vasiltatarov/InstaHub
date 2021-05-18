namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserSavedPostsService
    {
        Task<bool> AddAsync(string userId, int postId);

        IEnumerable<T> GetUserSavedPosts<T>(string userId);

        Task Delete(string userId, int postId);

        Task<bool> IsPostSaved(string userId, int postId);
    }
}

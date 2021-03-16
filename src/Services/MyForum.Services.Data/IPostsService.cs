namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPostsService
    {
        Task<T> GetById<T>(int id);

        Task<int> CreateAsync(string title, string content, int categoryId, string userId);

        IEnumerable<T> GetAllPosts<T>();

        IEnumerable<T> GetByCategoryId<T>(int categoryId, int? take = null, int skip = 0);

        int GetCountByCategoryId(int categoryId);

        Task IncreaseVisitorsCount(int id);
    }
}

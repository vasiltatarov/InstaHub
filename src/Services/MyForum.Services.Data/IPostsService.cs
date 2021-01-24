namespace MyForum.Services.Data
{
    using System.Threading.Tasks;

    public interface IPostsService
    {
        T GetById<T>(int id);

        Task<int> CreateAsync(string title, string content, int categoryId, string userId);
    }
}

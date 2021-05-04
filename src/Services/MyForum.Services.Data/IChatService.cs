namespace MyForum.Services.Data
{
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task CreateAsync(string message, string userId);
    }
}

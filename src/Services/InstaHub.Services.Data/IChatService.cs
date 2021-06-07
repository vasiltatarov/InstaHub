namespace InstaHub.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IChatService
    {
        Task CreateAsync(string message, string userId);

        IEnumerable<T> GetMessages<T>();
    }
}

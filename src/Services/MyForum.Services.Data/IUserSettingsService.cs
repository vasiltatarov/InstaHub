namespace MyForum.Services.Data
{
    using System.Threading.Tasks;

    public interface IUserSettingsService
    {
        Task AddDescription(string userId, string description);
    }
}

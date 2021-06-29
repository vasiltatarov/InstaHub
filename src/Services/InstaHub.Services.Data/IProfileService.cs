namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    using InstaHub.Web.ViewModels.Profile;

    public interface IProfileService
    {
        Task<PostInProfileViewModel> GetUserPosts(string username, string currentUserId, string followedUserId, string currentUserImagePath);
    }
}

namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    using InstaHub.Web.ViewModels.Profile;

    public interface IProfileService
    {
        Task<PostInProfileViewModel> GetUserPosts(string username, string currentUserId, string followedUserId, string currentUserImagePath);

        Task<FollowerInProfileViewModel> GetUserFollowers(string username, string currentUserId, string followedUserId, string currentUserImagePath);

        Task<FollowingInProfileViewModel> GetUserFollowing(string username, string currentUserId, string followedUserId, string currentUserImagePath);

        Task<AboutInProfileViewModel> GetUserAbout(string username, string currentUserId, string followedUserId, string currentUserImagePath);

        Task<PhotoInProfileViewModel> GetUserPhotos(string username, string currentUserId, string followedUserId, string currentUserImagePath);
    }
}

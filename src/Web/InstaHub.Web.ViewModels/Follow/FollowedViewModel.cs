namespace InstaHub.Web.ViewModels.Follow
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class FollowedViewModel : IMapFrom<UserFollow>
    {
        public string FollowedUserName { get; set; }

        public string FollowedImagePath { get; set; }
    }
}

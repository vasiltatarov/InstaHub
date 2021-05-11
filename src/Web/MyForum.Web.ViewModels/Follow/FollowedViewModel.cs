namespace MyForum.Web.ViewModels.Follow
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class FollowedViewModel : IMapFrom<UserFollow>
    {
        public string FollowedUserName { get; set; }

        public string FollowedImagePath { get; set; }
    }
}

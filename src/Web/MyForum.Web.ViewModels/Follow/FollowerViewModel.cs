namespace MyForum.Web.ViewModels.Follow
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class FollowerViewModel : IMapFrom<UserFollow>
    {
        public string FollowerUserName { get; set; }

        public string FollowerImagePath { get; set; }
    }
}

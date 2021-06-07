namespace InstaHub.Web.ViewModels.Follow
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class FollowerViewModel : IMapFrom<UserFollow>
    {
        public string FollowerUserName { get; set; }

        public string FollowerImagePath { get; set; }
    }
}

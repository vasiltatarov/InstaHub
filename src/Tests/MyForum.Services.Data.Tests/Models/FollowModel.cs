namespace MyForum.Services.Data.Tests.Models
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class FollowModel : IMapFrom<UserFollow>
    {
        public int Id { get; set; }

        public string FollowerId { get; set; }

        public string FollowedId { get; set; }

        public bool IsFollowActive { get; set; }
    }
}

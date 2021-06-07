namespace InstaHub.Services.Data.Tests.Models
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class FollowModel : IMapFrom<UserFollow>
    {
        public int Id { get; set; }

        public string FollowerId { get; set; }

        public string FollowedId { get; set; }

        public bool IsFollowActive { get; set; }
    }
}

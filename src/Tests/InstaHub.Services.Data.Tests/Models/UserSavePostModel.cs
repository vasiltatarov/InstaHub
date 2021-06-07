namespace InstaHub.Services.Data.Tests.Models
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class UserSavePostModel : IMapFrom<UserSavedPost>
    {
        public string UserId { get; set; }

        public int PostId { get; set; }
    }
}

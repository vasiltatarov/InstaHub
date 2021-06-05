namespace MyForum.Services.Data.Tests.Models
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class UserSavePostModel : IMapFrom<UserSavedPost>
    {
        public string UserId { get; set; }

        public int PostId { get; set; }
    }
}

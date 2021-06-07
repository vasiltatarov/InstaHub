namespace InstaHub.Services.Data.Tests.Models
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PostModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}

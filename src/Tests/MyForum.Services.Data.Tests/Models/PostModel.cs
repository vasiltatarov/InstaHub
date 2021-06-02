namespace MyForum.Services.Data.Tests.Models
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class PostModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}

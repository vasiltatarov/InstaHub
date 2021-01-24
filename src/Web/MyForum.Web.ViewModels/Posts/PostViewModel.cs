namespace MyForum.Web.ViewModels.Posts
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class PostViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string UserUserName { get; set; }
    }
}

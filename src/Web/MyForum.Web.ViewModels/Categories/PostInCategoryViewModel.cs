namespace MyForum.Web.ViewModels.Categories
{
    using System;

    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class PostInCategoryViewModel : IMapFrom<Post>
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string ShortContent
            => this.Content?.Length >= 250
                ? this.Content.Substring(0, 250) + "..."
                : this.Content;

        public string UserUserName { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

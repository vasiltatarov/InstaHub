namespace MyForum.Web.ViewModels.ViewUserProfile
{
    using System;
    using System.Collections.Generic;

    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class UserProfilePostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}

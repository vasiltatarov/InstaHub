namespace InstaHub.Web.ViewModels.ViewUserProfile
{
    using System;
    using System.Collections.Generic;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

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

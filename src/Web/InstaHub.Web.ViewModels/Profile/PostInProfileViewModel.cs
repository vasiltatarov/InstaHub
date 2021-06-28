namespace InstaHub.Web.ViewModels.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PostInProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public int Followers { get; set; }

        public int PostsCount { get; set; }

        [NotMapped]
        public int Followed { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }

        public IEnumerable<UserProfilePostViewModel> Posts { get; set; }
    }
}

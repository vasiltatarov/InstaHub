namespace InstaHub.Web.ViewModels.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.Follow;

    public class FollowerInProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int PostsCount { get; set; }

        [NotMapped]
        public int FollowedCount { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }

        [NotMapped]
        public IEnumerable<FollowerViewModel> Followers { get; set; }
    }
}

namespace InstaHub.Web.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations.Schema;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PhotoInProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int PostsCount { get; set; }

        [NotMapped]
        public int FollowersCount { get; set; }

        [NotMapped]
        public int FollowingsCount { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }
    }
}

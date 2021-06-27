namespace InstaHub.Web.ViewModels.Profile
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.Follow;
    using InstaHub.Web.ViewModels.Profiles;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImagePath { get; set; }

        public string Gender { get; set; }

        public string Description { get; set; }

        public int PostsCount { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }

        [NotMapped]
        public IEnumerable<FollowerViewModel> Followers { get; set; }

        [NotMapped]
        public IEnumerable<FollowedViewModel> Followed { get; set; }

        public IEnumerable<UserProfilePostViewModel> Posts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ApplicationUser, ProfileViewModel>()
                .ForMember(x => x.Username,
                    y => y.MapFrom(x => x.UserName));
        }
    }
}

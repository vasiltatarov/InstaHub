namespace MyForum.Web.ViewModels.ViewUserProfile
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels.Profiles;

    public class UserProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ImagePath { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }

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

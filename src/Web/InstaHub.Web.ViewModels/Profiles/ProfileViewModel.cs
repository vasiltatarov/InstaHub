﻿namespace InstaHub.Web.ViewModels.Profiles
{
    using System.Linq;

    using AutoMapper;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.ViewUserProfile;

    public class ProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string Location { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int PostsCount { get; set; }

        public int PostViews { get; set; }

        public int VotesCount { get; set; }

        public UserProfilePostViewModel[] Posts { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration
                .CreateMap<ApplicationUser, ProfileViewModel>()
                .ForMember(x => x.Username,
                    y => y.MapFrom(x => x.UserName))
                .ForMember(x => x.PostViews,
                    y => y.MapFrom(x => x.Posts.Sum(p => p.VisitorsCount)));
        }
    }
}

using System.Linq;

namespace MyForum.Web.ViewModels.Profiles
{

    using AutoMapper;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class ProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Username { get; set; }

        public string ImagePath { get; set; }

        public int PostsCount { get; set; }

        public int PostViews { get; set; }

        public int VotesCount { get; set; }

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

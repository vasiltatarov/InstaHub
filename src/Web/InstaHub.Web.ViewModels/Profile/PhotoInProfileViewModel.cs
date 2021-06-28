using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace InstaHub.Web.ViewModels.Profile
{
    using System.ComponentModel.DataAnnotations.Schema;

    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PhotoInProfileViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public int PostsCount { get; set; }

        public IEnumerable<string> Images { get; set; }

        [NotMapped]
        public int FollowersCount { get; set; }

        [NotMapped]
        public int FollowingsCount { get; set; }

        [NotMapped]
        public string CurrentUserImagePath { get; set; }

        [NotMapped]
        public bool IsUserFollowed { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, PhotoInProfileViewModel>()
                .ForMember(
                    x => x.Images,
                    y => y
                        .MapFrom(x => x.Posts
                            .Where(p => p.Content.Contains("<img src=\"") && p.Content.Length < 500)
                            .Select(p => p.Content)));
        }
    }
}

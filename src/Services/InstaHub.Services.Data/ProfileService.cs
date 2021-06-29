namespace InstaHub.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.Follow;
    using InstaHub.Web.ViewModels.Profile;
    using Microsoft.EntityFrameworkCore;

    public class ProfileService : IProfileService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IFollowService followService;

        public ProfileService(IDeletableEntityRepository<ApplicationUser> userRepository, IFollowService followService)
        {
            this.userRepository = userRepository;
            this.followService = followService;
        }

        public async Task<PostInProfileViewModel> GetUserPosts(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            var posts = await this.userRepository.All()
                .Where(x => x.UserName == username)
                .To<PostInProfileViewModel>()
                .FirstOrDefaultAsync();

            if (posts == null)
            {
                return null;
            }

            posts.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUserId, followedUserId);
            posts.Followers = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUserId).Count();
            posts.Followed = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUserId).Count();

            posts.CurrentUserImagePath = currentUserImagePath;

            return posts;
        }
    }
}

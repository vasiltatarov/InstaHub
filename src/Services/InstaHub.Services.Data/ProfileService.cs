namespace InstaHub.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
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

        public async Task<FollowerInProfileViewModel> GetUserFollowers(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            var followers = await this.userRepository.All()
                .Where(x => x.UserName == username)
                .To<FollowerInProfileViewModel>()
                .FirstOrDefaultAsync();

            if (followers == null)
            {
                return null;
            }

            followers.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUserId, followedUserId);
            followers.Followers = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUserId);
            followers.FollowedCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUserId).Count();
            followers.CurrentUserImagePath = currentUserImagePath;

            return followers;
        }

        public async Task<FollowingInProfileViewModel> GetUserFollowing(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            var following = await this.userRepository.All()
                .Where(x => x.UserName == username)
                .To<FollowingInProfileViewModel>()
                .FirstOrDefaultAsync();

            if (following == null)
            {
                return null;
            }

            following.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUserId, followedUserId);
            following.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUserId).Count();
            following.Followed = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUserId);
            following.CurrentUserImagePath = currentUserImagePath;

            return following;
        }

        public async Task<AboutInProfileViewModel> GetUserAbout(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            var about = await this.userRepository.All()
                .Where(x => x.UserName == username)
                .To<AboutInProfileViewModel>()
                .FirstOrDefaultAsync();

            about.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUserId, followedUserId);
            about.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUserId).Count();
            about.FollowingsCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUserId).Count();
            about.CurrentUserImagePath = currentUserImagePath;

            return about;
        }

        public async Task<PhotoInProfileViewModel> GetUserPhotos(string username, string currentUserId, string followedUserId, string currentUserImagePath)
        {
            var photos = await this.userRepository.All()
                .Where(x => x.UserName == username)
                .To<PhotoInProfileViewModel>()
                .FirstOrDefaultAsync();

            var imageList = new List<string>();
            var pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            var rx = new Regex(pattern);

            foreach (var image in photos.Images)
            {
                foreach (Match m in rx.Matches(image))
                {
                    var url = m.Groups["url"].Value;
                    imageList.Add(url);
                }
            }

            photos.Images = imageList;

            photos.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUserId, followedUserId);
            photos.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUserId).Count();
            photos.FollowingsCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUserId).Count();
            photos.CurrentUserImagePath = currentUserImagePath;

            return photos;
        }
    }
}

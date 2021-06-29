namespace InstaHub.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.Follow;
    using InstaHub.Web.ViewModels.Profile;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PagedList;

    using static InstaHub.Common.GlobalConstants;

    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFollowService followService;
        private readonly IProfileService profileService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IFollowService followService,
            IProfileService profileService)
        {
            this.userManager = userManager;
            this.followService = followService;
            this.profileService = profileService;
        }

        [Authorize]
        public async Task<IActionResult> GetPosts(string username, int page = DefaultPage) // Return two id's of the users
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var posts = await this.profileService.GetUserPosts(username, currentUserId, followedUserId, currentUserImagePath);

            if (posts == null)
            {
                return this.BadRequest();
            }

            posts.Posts = posts.Posts.ToPagedList(page, ItemsOnPaged);

            return this.View(posts);
        }

        public async Task<IActionResult> GetFollowers(string username)
        {
            var userViewModel = this.userManager.Users
                .To<FollowerInProfileViewModel>()
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUser.Id, followedUser.Id);
            userViewModel.Followers = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUser.Id);
            userViewModel.FollowedCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id).Count();

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            return this.View(userViewModel);
        }

        public async Task<IActionResult> GetFollowing(string username)
        {
            var userViewModel = this.userManager.Users
                .To<FollowingInProfileViewModel>()
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUser.Id, followedUser.Id);
            userViewModel.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUser.Id).Count();
            userViewModel.Followed = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id);

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            return this.View(userViewModel);
        }

        public async Task<IActionResult> About(string username)
        {
            var userViewModel = this.userManager.Users
                .To<AboutInProfileViewModel>()
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUser.Id, followedUser.Id);
            userViewModel.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUser.Id).Count();
            userViewModel.FollowingsCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id).Count();

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            return this.View(userViewModel);
        }

        public async Task<IActionResult> Photos(string username)
        {
            var userViewModel = this.userManager.Users
                .To<PhotoInProfileViewModel>()
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var imageList = new List<string>();
            var pattern = @"<img.*?src=""(?<url>.*?)"".*?>";
            var rx = new Regex(pattern);

            foreach (var image in userViewModel.Images)
            {
                foreach (Match m in rx.Matches(image))
                {
                    var url = m.Groups["url"].Value;
                    imageList.Add(url);
                }
            }

            userViewModel.Images = imageList;

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUser.Id, followedUser.Id);
            userViewModel.FollowersCount = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUser.Id).Count();
            userViewModel.FollowingsCount = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id).Count();

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            return this.View(userViewModel);
        }

        private async Task<(string T1, string T2, string T3)> GetUserIds(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            return (currentUser.Id, followedUser.Id, currentUser.ImagePath);
        }
    }
}

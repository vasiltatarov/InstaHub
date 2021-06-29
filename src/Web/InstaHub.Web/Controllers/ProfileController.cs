namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
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
        private readonly IProfileService profileService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IProfileService profileService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
        }

        [Authorize]
        public async Task<IActionResult> GetPosts(string username, int page = DefaultPage)
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var posts = await this.profileService.GetUserPosts(username, currentUserId, followedUserId, currentUserImagePath);

            posts.Posts = posts.Posts.ToPagedList(page, ItemsOnPaged);

            return this.View(posts);
        }

        public async Task<IActionResult> GetFollowers(string username)
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var followers = await this.profileService.GetUserFollowers(username, currentUserId, followedUserId, currentUserImagePath);

            return this.View(followers);
        }

        public async Task<IActionResult> GetFollowing(string username)
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var following = await this.profileService.GetUserFollowing(username, currentUserId, followedUserId, currentUserImagePath);

            return this.View(following);
        }

        public async Task<IActionResult> About(string username)
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var userAbout = await this.profileService.GetUserAbout(username, currentUserId, followedUserId, currentUserImagePath);

            return this.View(userAbout);
        }

        public async Task<IActionResult> Photos(string username)
        {
            var (currentUserId, followedUserId, currentUserImagePath) = await this.GetUserIds(username);
            var photos = await this.profileService.GetUserPhotos(username, currentUserId, followedUserId, currentUserImagePath);

            return this.View(photos);
        }

        private async Task<(string T1, string T2, string T3)> GetUserIds(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            return (currentUser.Id, followedUser.Id, currentUser.ImagePath);
        }
    }
}

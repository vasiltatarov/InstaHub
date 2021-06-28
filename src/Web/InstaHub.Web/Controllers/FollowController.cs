namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class FollowController : Controller
    {
        private readonly IFollowService followService;
        private readonly UserManager<ApplicationUser> userManager;

        public FollowController(IFollowService followService, UserManager<ApplicationUser> userManager)
        {
            this.followService = followService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Follow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            await this.followService.FollowAsync(currentUser.Id, followedUser.Id);

            return this.RedirectToAction("GetPosts", "Profile", new { username });
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            await this.followService.UnfollowAsync(currentUser.Id, followedUser.Id);

            return this.RedirectToAction("GetPosts", "Profile", new { username });
        }
    }
}

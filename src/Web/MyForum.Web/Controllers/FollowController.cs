namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyForum.Data.Models;
    using MyForum.Services.Data;

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

            await this.followService.Follow(currentUser.Id, followedUser.Id);

            return this.RedirectToAction("ByUsername", "ViewUserProfile", new { username });
        }

        [Authorize]
        public async Task<IActionResult> Unfollow(string username)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == username);

            await this.followService.Unfollow(currentUser.Id, followedUser.Id);

            return this.RedirectToAction("ByUsername", "ViewUserProfile", new { username });
        }
    }
}

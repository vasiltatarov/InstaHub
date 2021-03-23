namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.UserSavedPosts;

    public class UserSavedPostsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public UserSavedPostsController(UserManager<ApplicationUser> userManager, IUserSavedPostsService userSavedPostsService)
        {
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        public async Task<IActionResult> SavePostByUser(int postId)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.userSavedPostsService.AddAsync(userId, postId);

            return this.RedirectToAction("ById", "Posts", new { id = postId });
        }

        [Authorize]
        public IActionResult GetSavedPosts()
        {
            var userId = this.userManager.GetUserId(this.User);
            var posts = this.userSavedPostsService.GetUserSavedPosts<SavePostViewModel>(userId);

            return this.View(posts);
        }

        //Drop SavedPost
    }
}

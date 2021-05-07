namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.UserSavedPosts;
    using PagedList;

    [Authorize]
    public class UserSavedPostsController : Controller
    {
        private const int ItemsOnPaged = 5;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public UserSavedPostsController(UserManager<ApplicationUser> userManager, IUserSavedPostsService userSavedPostsService)
        {
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        public IActionResult GetSavedPosts(int page = 1)
        {
            var userId = this.userManager.GetUserId(this.User);
            var posts = this.userSavedPostsService
                .GetUserSavedPosts<SavePostViewModel>(userId)
                .ToPagedList(page, ItemsOnPaged);

            return this.View(posts);
        }

        [Authorize]
        public async Task<IActionResult> DeleteSavedPostById(int postId)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.userSavedPostsService.Delete(userId, postId);

            this.TempData["InfoMessage"] = "Deleted Successfully";

            return this.RedirectToAction("GetSavedPosts");
        }
    }
}

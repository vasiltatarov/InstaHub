﻿namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.UserSavedPosts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PagedList;

    using static InstaHub.Common.GlobalConstants;

    [Authorize]
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
        public IActionResult GetSavedPosts(int page = DefaultPage)
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

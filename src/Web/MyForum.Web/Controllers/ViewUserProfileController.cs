﻿namespace MyForum.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels.Follow;
    using MyForum.Web.ViewModels.ViewUserProfile;
    using PagedList;

    [Authorize]
    public class ViewUserProfileController : Controller
    {
        private const int PagedOnList = 5;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFollowService followService;

        public ViewUserProfileController(
            UserManager<ApplicationUser> userManager,
            IFollowService followService)
        {
            this.userManager = userManager;
            this.followService = followService;
        }

        [Authorize]
        public async Task<IActionResult> ByUsername(string username, int page = 1)
        {
            var userViewModel = this.userManager.Users
                .To<UserProfileViewModel>()
                .FirstOrDefault(x => x.Username == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExist(currentUser.Id, followedUser.Id);
            userViewModel.Followers = this.followService.GetFollowersByUserId<FollowerViewModel>(currentUser.Id);
            userViewModel.Followed = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id);

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            userViewModel.Posts = userViewModel.Posts.ToPagedList(page, PagedOnList);

            return this.View(userViewModel);
        }
    }
}

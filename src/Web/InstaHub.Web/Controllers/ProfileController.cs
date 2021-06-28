﻿namespace InstaHub.Web.Controllers
{
    using System.Linq;
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

    [Authorize]
    public class ProfileController : Controller
    {
        private const int PagedOnList = 5;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFollowService followService;

        public ProfileController(
            UserManager<ApplicationUser> userManager,
            IFollowService followService)
        {
            this.userManager = userManager;
            this.followService = followService;
        }

        [Authorize]
        public async Task<IActionResult> GetPosts(string username, int page = 1)
        {
            var userViewModel = this.userManager.Users
                .To<PostInProfileViewModel>()
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = await this.userManager.Users
                .FirstOrDefaultAsync(x => x.UserName == username);

            userViewModel.IsUserFollowed = await this.followService.CheckIfFollowExistAsync(currentUser.Id, followedUser.Id);
            userViewModel.Followers = this.followService.GetFollowersByUserId<FollowerViewModel>(followedUser.Id).Count();
            userViewModel.Followed = this.followService.GetFollowedByUserId<FollowedViewModel>(followedUser.Id).Count();

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            userViewModel.Posts = userViewModel.Posts.ToPagedList(page, PagedOnList);

            return this.View(userViewModel);
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
    }
}

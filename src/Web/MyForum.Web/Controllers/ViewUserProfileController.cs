namespace MyForum.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels.ViewUserProfile;
    using PagedList;

    [Authorize]
    public class ViewUserProfileController : Controller
    {
        private const int PagedOnList = 5;

        private readonly UserManager<ApplicationUser> userManager;

        public ViewUserProfileController(
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> ByUsername(string username, int page = 1)
        {
            var userViewModel = this.userManager.Users
                .To<UserProfileViewModel>()
                .FirstOrDefault(x => x.Username == username);

            var currentUser = await this.userManager.GetUserAsync(this.User);
            var followedUser = this.userManager.Users
                .FirstOrDefault(x => x.UserName == username);

            if (userViewModel == null)
            {
                return this.BadRequest();
            }

            userViewModel.CurrentUserImagePath = currentUser.ImagePath;

            userViewModel.Posts = userViewModel.Posts.ToPagedList(page, PagedOnList);

            return this.View(userViewModel);
        }
    }
}

using System.Security.Claims;
using System.Threading.Tasks;

namespace MyForum.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels.ViewUserProfile;
    using PagedList;

    public class ViewUserProfileController : Controller
    {
        private const int PagedOnList = 5;

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<Post> postRepository;

        public ViewUserProfileController(
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<Post> postRepository)
        {
            this.userManager = userManager;
            this.postRepository = postRepository;
        }

        [Authorize]
        public IActionResult ByUsername(string username, int page = 1)
        {
            var user = this.userManager.Users
                .To<UserProfileViewModel>()
                .FirstOrDefault(x => x.Username == username);

            var currentUserImagePath = this.userManager.GetUserAsync(this.User).Result.ImagePath;

            if (user == null)
            {
                return this.BadRequest();
            }

            user.CurrentUserImagePath = currentUserImagePath;

            user.Posts = user.Posts.ToPagedList(page, PagedOnList);

            return this.View(user);
        }
    }
}

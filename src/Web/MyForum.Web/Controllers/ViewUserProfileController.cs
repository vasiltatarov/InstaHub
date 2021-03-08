using Microsoft.AspNetCore.Authorization;
using MyForum.Data.Common.Repositories;

namespace MyForum.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Web.ViewModels.ViewUserProfile;

    public class ViewUserProfileController : Controller
    {
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
        public IActionResult ByUsername(string username)
        {
            var user = this.userManager.Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                return this.BadRequest();
            }
            //The Service will acept data - username, CreatedOn, ...
            var userViewModel = new UserProfileViewModel()
            {
                Username = user.UserName,
                CreatedOn = user.CreatedOn,
                ImagePath = user.ImagePath,
                Posts = this.postRepository.All()
                    .Where(x => x.UserId == user.Id)
                    .ToList(),
            };

            return this.View(userViewModel);
        }
    }
}

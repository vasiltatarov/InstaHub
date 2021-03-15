namespace MyForum.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Common.Repositories;
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

            var userViewModel = new UserProfileViewModel()
            {
                Username = user.UserName,
                CreatedOn = user.CreatedOn,
                ImagePath = user.ImagePath,
                Posts = this.postRepository.All()
                    .Where(x => x.UserId == user.Id)
                    .Select(x => new UserProfilePostViewModel()
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Content = x.Content,
                        CreatedOn = x.CreatedOn,
                        Comments = x.Comments,
                        Votes = x.Votes,
                    })
                    .ToArray(),
            };

            return this.View(userViewModel);
        }
    }
}

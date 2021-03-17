namespace MyForum.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;
    using MyForum.Web.ViewModels.Profiles;

    public class ProfilesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ProfilesController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult GetProfiles()
            => this.View(this.userManager
                .Users
                .To<ProfileViewModel>()
                .ToList());
    }
}

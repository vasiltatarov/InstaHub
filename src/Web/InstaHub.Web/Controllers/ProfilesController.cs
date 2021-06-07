namespace InstaHub.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using InstaHub.Web.ViewModels.Profiles;

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

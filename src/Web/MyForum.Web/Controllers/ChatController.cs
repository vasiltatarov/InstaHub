namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;

    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Chat() => this.View(await this.userManager.GetUserAsync(this.User));
    }
}

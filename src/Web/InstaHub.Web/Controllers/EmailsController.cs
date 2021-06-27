namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Services.Messaging;
    using InstaHub.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class EmailsController : Controller
    {
        private readonly IPostsService postsService;
        private readonly IEmailSender emailSender;
        private readonly UserManager<ApplicationUser> userManager;

        public EmailsController(IPostsService postsService, IEmailSender emailSender, UserManager<ApplicationUser> userManager)
        {
            this.postsService = postsService;
            this.emailSender = emailSender;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendPostToEmail(int id)
        {
            var post = await this.postsService.GetById<PostViewModel>(id);
            var user = await this.userManager.GetUserAsync(this.User);

            await this.emailSender
                .SendEmailAsync("vasiltatarov3@gmail.com", "Vasko", "vasiltatarov3@gmail.com", post.Title, post.Content);

            return this.RedirectToAction("ById", "Posts", new { id });
        }
    }
}

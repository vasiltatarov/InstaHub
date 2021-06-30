namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Services.Messaging;
    using InstaHub.Web.ViewModels.Emails;
    using InstaHub.Web.ViewModels.Posts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
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

        public IActionResult SendPostToEmail() => this.View();

        [HttpPost]
        public async Task<IActionResult> SendPostToEmail(SendFormViewModel form)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(form);
            }

            var post = await this.postsService.GetById<PostViewModel>(form.Id);
            var user = await this.userManager.GetUserAsync(this.User);

            await this.emailSender
                .SendEmailAsync(user.Email, user.UserName, form.To, post.Title, post.Content);

            return this.RedirectToAction("ById", "Posts", new { form.Id });
        }
    }
}

namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Chat;

    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IChatService chatService;

        public ChatController(UserManager<ApplicationUser> userManager, IChatService chatService)
        {
            this.userManager = userManager;
            this.chatService = chatService;
        }

        [Authorize]
        public async Task<IActionResult> Chat()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var userMessages = new UserMessages
            {
                UserName = user.UserName,
                UserImagePath = user.ImagePath,
                Messages = this.chatService.GetMessages<Message>(),
            };

            return this.View(userMessages);
        }
    }
}

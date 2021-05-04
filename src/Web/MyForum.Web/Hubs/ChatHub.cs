namespace MyForum.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using MyForum.Data.Models;
    using MyForum.Services.Data;
    using MyForum.Web.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IChatService chatService;

        public ChatHub(UserManager<ApplicationUser> userManager, IChatService chatService)
        {
            this.userManager = userManager;
            this.chatService = chatService;
        }

        public async Task Send(string message)
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);
            await this.chatService.CreateAsync(message, user.Id);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new Message
                {
                    UserUserName = user.UserName,
                    UserImagePath = user.ImagePath,
                    Text = message,
                    CreatedOn = DateTime.UtcNow,
                });
        }
    }
}

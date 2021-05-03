namespace MyForum.Web.Hubs
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using MyForum.Data.Models;
    using MyForum.Web.ViewModels.Chat;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Send(string message)
        {
            var user = await this.userManager.GetUserAsync(this.Context.User);

            await this.Clients.All.SendAsync(
                "NewMessage",
                new Message
                {
                    UserImagePath = user.ImagePath,
                    Text = message,
                    CreatedOn = DateTime.UtcNow,
                });
        }
    }
}

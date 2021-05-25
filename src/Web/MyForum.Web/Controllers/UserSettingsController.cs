﻿namespace MyForum.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using MyForum.Data.Models;
    using MyForum.Services.Data;

    [ApiController]
    [Route("api/[controller]")]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUserSettingsService userSettings;
        private readonly UserManager<ApplicationUser> userManager;

        public UserSettingsController(IUserSettingsService userSettings, UserManager<ApplicationUser> userManager)
        {
            this.userSettings = userSettings;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet("changeDescription/{content}")]
        public async Task ChangeDescription(string content)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            await this.userSettings.AddDescription(user.Id, content);
        }
    }
}

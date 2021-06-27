namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.UserSavePost;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class UserSavePostController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserSavedPostsService userSavedPostsService;

        public UserSavePostController(UserManager<ApplicationUser> userManager, IUserSavedPostsService userSavedPostsService)
        {
            this.userManager = userManager;
            this.userSavedPostsService = userSavedPostsService;
        }

        [Authorize]
        [HttpGet("save/{postId}")]
        public async Task<ActionResult<SavePostResponseModel>> Save(int postId)
        {
            var message = "Saved Successfully!";
            var userId = this.userManager.GetUserId(this.User);
            var result = await this.userSavedPostsService.AddAsync(userId, postId);

            if (!result)
            {
                message = "It is already saved!";
            }

            return new SavePostResponseModel() { Message = message };
        }

        // [Authorize]
        // [HttpGet("delete/{postId}")]
        // public async Task Delete(int postId)
        // {
        //    var userId = this.userManager.GetUserId(this.User);
        //    await this.userSavedPostsService.Delete(userId, postId);
        // }
    }
}

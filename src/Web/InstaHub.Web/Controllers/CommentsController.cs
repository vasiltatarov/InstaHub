namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Comments;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly UserManager<ApplicationUser> userManager;

        public CommentsController(ICommentService commentService, UserManager<ApplicationUser> userManager)
        {
            this.commentService = commentService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CreateCommentInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            int? parentId = input.ParentId == 0
                ? null
                : input.ParentId;

            if (parentId.HasValue)
            {
                if (this.commentService.IsInPostId(parentId.Value, input.PostId))
                {
                    this.BadRequest();
                }
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.commentService.CreateComment(input.PostId, userId, input.Content, parentId);

            return this.RedirectToAction("ById", "Posts", new { id = input.PostId });
        }
    }
}

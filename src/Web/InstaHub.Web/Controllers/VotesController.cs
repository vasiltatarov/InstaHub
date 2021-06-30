namespace InstaHub.Web.Controllers
{
    using System.Threading.Tasks;

    using InstaHub.Data.Models;
    using InstaHub.Services.Data;
    using InstaHub.Web.ViewModels.Votes;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService voteService;
        private readonly UserManager<ApplicationUser> userManager;

        public VotesController(IVoteService voteService, UserManager<ApplicationUser> userManager)
        {
            this.voteService = voteService;
            this.userManager = userManager;
        }

        /// <summary>
        /// Example
        /// POST /api/votes
        /// Request body: {"postId":1, "isUpVote":true}
        /// Response body: {"votesCount":15}.
        /// </summary>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<VoteResponseModel>> Post(VoteInputModel input)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.voteService.VoteAsync(input.PostId, userId, input.IsUpVote);
            var votes = this.voteService.GetVotes(input.PostId);

            return new VoteResponseModel() { VotesCount = votes };
        }
    }
}

namespace InstaHub.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;

    public class VoteService : IVoteService
    {
        private readonly IRepository<Vote> votesRepository;

        public VoteService(IRepository<Vote> votesRepository)
            => this.votesRepository = votesRepository;

        /// <summary>
        /// VoteAsync - Create vote and add it to db. If vote is already exist, just update it.
        /// </summary>
        /// <param name="isUpVote">If isUpVote is true - Then UpVote Else DownVote.</param>
        /// <returns></returns>
        public async Task VoteAsync(int postId, string userId, bool isUpVote)
        {
            var vote = this.votesRepository.All()
                .FirstOrDefault(x => x.PostId == postId && x.UserId == userId);

            if (vote == null)
            {
                vote = new Vote()
                {
                    PostId = postId,
                    UserId = userId,
                    Type = isUpVote ? VoteType.UpVote : VoteType.DownVote,
                };

                await this.votesRepository.AddAsync(vote);
            }
            else
            {
                vote.Type = isUpVote ? VoteType.UpVote : VoteType.DownVote;
            }

            await this.votesRepository.SaveChangesAsync();
        }

        /// Get Rating (all up votes and down votes)
        public int GetVotes(int postId)
            => this.votesRepository.All()
                .Where(x => x.PostId == postId)
                .Sum(x => (int)x.Type);
    }
}

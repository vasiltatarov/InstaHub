﻿namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    public interface IVoteService
    {
        Task VoteAsync(int postId, string userId, bool isUpVote);

        int GetVotes(int postId);
    }
}

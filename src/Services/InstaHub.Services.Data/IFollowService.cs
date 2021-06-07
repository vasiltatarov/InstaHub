namespace InstaHub.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFollowService
    {
        Task FollowAsync(string followerId, string followedId);

        Task UnfollowAsync(string followerId, string followedId);

        IEnumerable<T> GetFollowersByUserId<T>(string userId);

        IEnumerable<T> GetFollowedByUserId<T>(string userId);

        Task<bool> CheckIfFollowExistAsync(string followerId, string followedId);
    }
}

namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IFollowService
    {
        Task Follow(string followerId, string followedId);

        Task Unfollow(string followerId, string followedId);

        IEnumerable<T> GetFollowersByUserId<T>(string userId);

        IEnumerable<T> GetFollowedByUserId<T>(string userId);

        Task<bool> CheckIfFollowExist(string followerId, string followedId);
    }
}

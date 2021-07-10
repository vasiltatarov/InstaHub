namespace InstaHub.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class FollowService : IFollowService
    {
        private readonly IRepository<UserFollow> userFollows;
        private readonly IRepository<ApplicationUser> userRepository;

        public FollowService(IRepository<UserFollow> userFollows, IRepository<ApplicationUser> userRepository)
        {
            this.userFollows = userFollows;
            this.userRepository = userRepository;
        }

        public async Task FollowAsync(string followerId, string followedId)
        {
            var userFollow = await this.userFollows.All()
                .FirstOrDefaultAsync(x => x.FollowerId == followerId && x.FollowedId == followedId);

            if (userFollow != null && userFollow.IsFollowActive)
            {
                return;
            }

            if (userFollow != null)
            {
                userFollow.IsFollowActive = true;
            }
            else
            {
                userFollow = new UserFollow
                {
                    FollowerId = followerId,
                    FollowedId = followedId,
                    IsFollowActive = true,
                };
                await this.userFollows.AddAsync(userFollow);
            }

            await this.userFollows.SaveChangesAsync();
        }

        public async Task UnfollowAsync(string followerId, string followedId)
        {
            var userFollow = await this.userFollows.All()
                .FirstOrDefaultAsync(x => x.FollowerId == followerId && x.FollowedId == followedId && x.IsFollowActive);

            if (userFollow != null)
            {
                userFollow.IsFollowActive = false;
                await this.userFollows.SaveChangesAsync();
            }
        }

        public IEnumerable<T> GetFollowersByUserId<T>(string userId)
            => this.userFollows.All()
                .Where(x => x.FollowedId == userId && x.IsFollowActive)
                .To<T>()
                .ToList();

        public IEnumerable<T> GetFollowedByUserId<T>(string userId)
            => this.userFollows.All()
                .Where(x => x.FollowerId == userId && x.IsFollowActive)
                .To<T>()
                .ToList();

        public async Task<bool> CheckIfFollowExistAsync(string followerId, string followedId)
            => await this.userFollows
                .All()
                .AnyAsync(x => x.FollowerId == followerId &&
                               x.FollowedId == followedId &&
                               x.IsFollowActive);
    }
}

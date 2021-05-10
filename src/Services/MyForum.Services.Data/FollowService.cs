namespace MyForum.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;

    public class FollowService : IFollowService
    {
        private readonly IRepository<UserFollow> userFollows;
        private readonly IRepository<ApplicationUser> useRepository;

        public FollowService(IRepository<UserFollow> userFollows, IRepository<ApplicationUser> useRepository)
        {
            this.userFollows = userFollows;
            this.useRepository = useRepository;
        }

        public async Task Follow(string followerId, string followedId)
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
                    Follower = await this.useRepository.All().FirstOrDefaultAsync(x => x.Id == followerId),
                    FollowedId = followedId,
                    Followed = await this.useRepository.All().FirstOrDefaultAsync(x => x.Id == followedId),
                    IsFollowActive = true,
                };
                await this.userFollows.AddAsync(userFollow);
            }

            await this.userFollows.SaveChangesAsync();
        }

        public async Task Unfollow(string followerId, string followedId)
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
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetFollowingByUserId<T>(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CheckIfFollowExist(string followerId, string followedId)
            => await this.userFollows.All()
                .AnyAsync(x => x.FollowerId == followerId &&
                               x.FollowedId == followedId &&
                               x.IsFollowActive);
    }
}

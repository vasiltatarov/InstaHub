﻿namespace MyForum.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class UserSavedPostsService : IUserSavedPostsService
    {
        private readonly IDeletableEntityRepository<UserSavedPost> userSavedPostsRepository;
        private readonly IDeletableEntityRepository<Post> postsRepository;

        public UserSavedPostsService(
            IDeletableEntityRepository<UserSavedPost> userSavedPostsRepository,
            IDeletableEntityRepository<Post> postsRepository)
        {
            this.userSavedPostsRepository = userSavedPostsRepository;
            this.postsRepository = postsRepository;
        }

        public async Task AddAsync(string userId, int postId)
        {
            if (this.userSavedPostsRepository.All()
                .Any(x => x.UserId == userId &&
                          x.PostId == postId &&
                          x.IsDeleted == false))
            {
                return;
            }

            var userSavePost = new UserSavedPost()
            {
                UserId = userId,
                PostId = postId,
                CreatedOn = DateTime.UtcNow,
            };

            await this.userSavedPostsRepository.AddAsync(userSavePost);
            await this.userSavedPostsRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetUserSavedPosts<T>(string userId)
            => this.userSavedPostsRepository
                .All()
                .Where(x => x.UserId == userId)
                .To<T>()
                .ToList();

        public async Task Delete(string userId, int postId)
        {
            var userPost = this.userSavedPostsRepository.All()
                .FirstOrDefault(x => x.UserId == userId && x.PostId == postId && x.IsDeleted == false);
            this.userSavedPostsRepository.Delete(userPost);
            await this.userSavedPostsRepository.SaveChangesAsync();
        }
    }
}

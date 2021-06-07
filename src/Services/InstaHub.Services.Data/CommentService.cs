namespace InstaHub.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;

    public class CommentService : ICommentService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;

        public CommentService(IDeletableEntityRepository<Comment> commentsRepository)
        {
            this.commentsRepository = commentsRepository;
        }

        public async Task CreateComment(int postId, string userId, string content, int? parentId = null)
        {
            var comment = new Comment()
            {
                Content = content,
                UserId = userId,
                PostId = postId,
                ParentId = parentId,
            };

            await this.commentsRepository.AddAsync(comment);
            await this.commentsRepository.SaveChangesAsync();
        }

        public bool IsInPostId(int commentId, int postId)
            => this.commentsRepository.All()
                .Where(x => x.Id == commentId)
                .Select(x => x.PostId)
                .FirstOrDefault() == postId;
    }
}

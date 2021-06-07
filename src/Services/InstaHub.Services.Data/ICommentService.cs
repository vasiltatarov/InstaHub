namespace InstaHub.Services.Data
{
    using System.Threading.Tasks;

    public interface ICommentService
    {
        Task CreateComment(int postId, string userId, string content, int? parentId = null);

        bool IsInPostId(int commentId, int postId);
    }
}

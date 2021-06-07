namespace InstaHub.Web.ViewModels.Votes
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// If IsUpVote is true - Then UpVote Else DownVote.
    /// </summary>
    public class VoteInputModel
    {
        [Required]
        public int PostId { get; set; }

        public bool IsUpVote { get; set; }
    }
}

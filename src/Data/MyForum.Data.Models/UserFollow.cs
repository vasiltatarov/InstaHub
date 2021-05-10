namespace MyForum.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserFollow
    {
        [Required]
        public string FollowerId { get; set; }

        public virtual ApplicationUser Follower { get; set; }

        [Required]
        public string FollowedId { get; set; }

        public virtual ApplicationUser Followed { get; set; }

        [Required]
        public bool IsFollowActive { get; set; }
    }
}

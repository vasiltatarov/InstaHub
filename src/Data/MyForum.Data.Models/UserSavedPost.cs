namespace MyForum.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MyForum.Data.Common.Models;

    public class UserSavedPost : BaseDeletableModel<int>
    {
        public UserSavedPost()
        {
            this.SavedPosts = new HashSet<Post>();
        }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Post> SavedPosts { get; set; }
    }
}

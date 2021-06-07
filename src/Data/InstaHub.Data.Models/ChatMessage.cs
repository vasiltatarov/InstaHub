namespace InstaHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using InstaHub.Data.Common.Models;

    public class ChatMessage : BaseDeletableModel<int>
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}

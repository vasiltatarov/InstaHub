namespace InstaHub.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using InstaHub.Data.Common.Models;

    public class ContactForm : BaseModel<int>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Content { get; set; }

        public string Ip { get; set; }
    }
}

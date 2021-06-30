namespace InstaHub.Web.ViewModels.Emails
{
    using System.ComponentModel.DataAnnotations;

    public class SendFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string To { get; set; }
    }
}

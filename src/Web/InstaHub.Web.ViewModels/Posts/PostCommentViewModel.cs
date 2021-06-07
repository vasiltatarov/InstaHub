namespace InstaHub.Web.ViewModels.Posts
{
    using System;

    using Ganss.XSS;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PostCommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserUserName { get; set; }

        public string UserImagePath { get; set; }

        public string Content { get; set; }

        public string SanitizeContent => new HtmlSanitizer().Sanitize(this.Content);
    }
}

namespace InstaHub.Web.ViewModels.UserSavedPosts
{
    using System;

    using AngleSharp.Dom;
    using AngleSharp.Html.Parser;
    using Ganss.XSS;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class SavePostViewModel : IMapFrom<UserSavedPost>
    {
        public int PostId { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.PostContent);

        public string ShortContent => new HtmlParser().ParseDocument(
                this.SanitizedContent.Length >= 250
                    ? this.SanitizedContent.Substring(0, 250) + "..."
                    : this.SanitizedContent)
            .Body
            .Text();

        public string PostUserUserName { get; set; }

        public string PostUserImagePath { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

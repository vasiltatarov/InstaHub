namespace InstaHub.Web.ViewModels.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AngleSharp.Dom;
    using AngleSharp.Html.Parser;
    using AutoMapper;
    using Ganss.XSS;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class PostViewModel : IMapFrom<Post>, IMapTo<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string SanitizedContent => new HtmlSanitizer().Sanitize(this.Content);

        public string ShortContent => new HtmlParser().ParseDocument(
            this.SanitizedContent.Length >= 250
            ? this.SanitizedContent.Substring(0, 250) + "..."
            : this.SanitizedContent)
            .Body
            .Text();

        public string UserUserName { get; set; }

        public string UserImagePath { get; set; }

        public DateTime CreatedOn { get; set; }

        public int VotesCount { get; set; }

        public int VisitorsCount { get; set; }

        public IEnumerable<PostCommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(x => x.VotesCount, options =>
                {
                    options.MapFrom(p => p.Votes.Sum(v => (int)v.Type));
                });
        }
    }
}

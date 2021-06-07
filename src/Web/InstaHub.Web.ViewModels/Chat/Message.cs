namespace InstaHub.Web.ViewModels.Chat
{
    using System;

    using AutoMapper;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class Message : IMapFrom<ChatMessage>, IHaveCustomMappings
    {
        public string Text { get; set; }

        public string UserUserName { get; set; }

        public string UserImagePath { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration) =>
            configuration.CreateMap<ChatMessage, Message>()
                .ForMember(
                    x => x.Text,
                    y => y.MapFrom(x => x.Message));
    }
}

namespace MyForum.Services.Data.Tests.Models
{
    using MyForum.Data.Models;
    using MyForum.Services.Mapping;

    public class ChatModel : IMapFrom<ChatMessage>
    {
        public string Message { get; set; }

        public string UserId { get; set; }
    }
}

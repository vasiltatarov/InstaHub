namespace InstaHub.Services.Data.Tests.Models
{
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class ChatModel : IMapFrom<ChatMessage>
    {
        public string Message { get; set; }

        public string UserId { get; set; }
    }
}

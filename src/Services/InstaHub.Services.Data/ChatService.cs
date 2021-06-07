namespace InstaHub.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using InstaHub.Data.Common.Repositories;
    using InstaHub.Data.Models;
    using InstaHub.Services.Mapping;

    public class ChatService : IChatService
    {
        private readonly IDeletableEntityRepository<ChatMessage> chatMessageRepository;

        public ChatService(IDeletableEntityRepository<ChatMessage> chatMessageRepository)
        {
            this.chatMessageRepository = chatMessageRepository;
        }

        public async Task CreateAsync(string message, string userId)
        {
            var chatMessage = new ChatMessage
            {
                Message = message,
                UserId = userId,
            };

            await this.chatMessageRepository.AddAsync(chatMessage);
            await this.chatMessageRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetMessages<T>()
            => this.chatMessageRepository
                .All()
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToList();
    }
}

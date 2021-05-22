namespace MyForum.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using MyForum.Data.Common.Repositories;
    using MyForum.Data.Models;
    using Xunit;

    public class VotesServiceTests
    {
        [Fact]
        public async Task WhenUsersVotesCountVotesMustBeCorrect()
        {
            var list = new List<Vote>();

            var mockRepo = new Mock<IRepository<Vote>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable());
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepo.Object);

            await service.VoteAsync(1, "v1", true);
            await service.VoteAsync(1, "v2", true);
            await service.VoteAsync(1, "v3", true);

            Assert.Equal(3, mockRepo.Object.All().Count());
        }

        [Fact]
        public async Task WhenUserVotesSecondTimeOnSamePostVotesCountMustBeCorrect()
        {
            var currentPostId = 1;

            var list = new List<Vote>();

            var mockRepo = new Mock<IRepository<Vote>>();
            mockRepo.Setup(x => x.All())
                .Returns(list.AsQueryable);
            mockRepo.Setup(x => x.AddAsync(It.IsAny<Vote>()))
                .Callback((Vote vote) => list.Add(vote));

            var service = new VotesService(mockRepo.Object);

            await service.VoteAsync(1, "v1", true);
            await service.VoteAsync(1, "v2", true);
            await service.VoteAsync(1, "v2", false);

            Assert.Equal(1, service.GetVotes(currentPostId));
        }
    }
}

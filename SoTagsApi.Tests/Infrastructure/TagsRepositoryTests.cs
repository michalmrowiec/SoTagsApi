using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SoTagsApi.Infrastructure.Repositories;
using SoTagsApi.Tests.Infrastructure.Helper;

namespace SoTagsApi.Tests.Infrastructure
{
    public class TagsRepositoryTests
    {
        [Fact]
        public async Task GetPaginatedSortedTags_ForValidData_ReturnsCorrectNumberOfTags()
        {
            await using var context = TestApplicationDbContextInMemoryFactory.Create();

            await context
                .Set<Tag>()
                .AddRangeAsync(
                    [
                        new Tag { Name = "c#", Count = 90, PercentageShare = 0.9 },
                        new Tag { Name = "js", Count = 10, PercentageShare = 0.1 }
                    ]);

            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<TagsRepository>>();

            TagsRepository tagsRepository = new(context, logger.Object);

            var response = await tagsRepository.GetPaginedSortedTags("percentageshare","desc");

            response.Should().NotBeNull();
            response.Items.Should().HaveCount(2);
        }

    }
}
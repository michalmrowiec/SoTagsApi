using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SoTagsApi.Domain.Models;
using SoTagsApi.Infrastructure.Repositories;
using SoTagsApi.Tests.UnitTests.Infrastructure.Helper;

namespace SoTagsApi.Tests.UnitTests.Infrastructure
{
    public class TagsRepositoryTests
    {
        public static IEnumerable<object[]> ValidDataForGetPaginatedSortedTags => new List<object[]>
        {
            new object[]
            {
                new List<Tag>(),
                1,
                1,
                "",
                "",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            },
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 90, PercentageShare = 0.9 },
                        new Tag { Id = 2, Name = "js", Count = 10, PercentageShare = 0.1 }
                },
                10,
                1,
                "percentageshare",
                "desc",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>()
                    {
                        new Tag { Id = 1, Name = "c#", Count = 90, PercentageShare = 0.9 },
                        new Tag { Id = 2, Name = "js", Count = 10, PercentageShare = 0.1 }
                    },
                    ItemsFrom = 1,
                    ItemsTo = 10,
                    TotalItems = 2,
                    TotalPages = 1,
                }
            }
        };

        [Theory]
        [MemberData(nameof(ValidDataForGetPaginatedSortedTags))]
        public async Task GetPaginatedSortedTags_ForValidData_ReturnsPagedSortedTags(
            List<Tag> tags,
            int pageSize,
            int pageNumber,
            string sortProperty,
            string sortOrder,
            PagedResult<Tag> expectedResponse)
        {
            await using var context = TestApplicationDbContextInMemoryFactory.Create();

            await context
                .Set<Tag>()
                .AddRangeAsync(tags);

            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<TagsRepository>>();

            TagsRepository tagsRepository = new(context, logger.Object);

            var response = await tagsRepository.GetPaginedSortedTags(sortProperty, sortOrder, pageSize, pageNumber);

            response.Should().BeEquivalentTo(expectedResponse);
        }

        public static IEnumerable<object[]> InalidDataForGetPaginatedSortedTags => new List<object[]>
        {
            new object[]
            {
                new List<Tag>(),
                0,
                1,
                "",
                "",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            },
            new object[]
            {
                new List<Tag>(),
                10,
                0,
                "",
                "",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            },
            new object[]
            {
                new List<Tag>(),
                101,
                1,
                "",
                "",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            },
            new object[]
            {
                new List<Tag>(),
                10,
                10_001,
                "",
                "",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            },
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 90, PercentageShare = 0.9 },
                        new Tag { Id = 2, Name = "js", Count = 10, PercentageShare = 0.1 }
                },
                10,
                1,
                "jyghfc",
                "desc",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>(),
                    ItemsFrom = 0,
                    ItemsTo = 0,
                    TotalItems = 0,
                    TotalPages = 0,
                }
            }
        };

        [Theory]
        [MemberData(nameof(InalidDataForGetPaginatedSortedTags))]
        public async Task GetPaginatedSortedTags_ForInvalidData_ReturnsEmptyPagedResult(
            List<Tag> tags,
            int pageSize,
            int pageNumber,
            string sortProperty,
            string sortOrder,
            PagedResult<Tag> expectedResponse)
        {
            await using var context = TestApplicationDbContextInMemoryFactory.Create();

            await context
                .Set<Tag>()
                .AddRangeAsync(tags);

            await context.SaveChangesAsync();

            var logger = new Mock<ILogger<TagsRepository>>();

            TagsRepository tagsRepository = new(context, logger.Object);

            var response = await tagsRepository.GetPaginedSortedTags(sortProperty, sortOrder, pageSize, pageNumber);

            response.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
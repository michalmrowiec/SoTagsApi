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
            },
            new object[]
            {
                new List<Tag>()
                {
                    new Tag { Id = 1, Name = "javascript", Count = 2528869, PercentageShare = 0.0767 },
                    new Tag { Id = 2, Name = "python", Count = 2192349, PercentageShare = 0.0665 },
                    new Tag { Id = 3, Name = "java", Count = 1917308, PercentageShare = 0.0582 },
                    new Tag { Id = 4, Name = "c#", Count = 1615064, PercentageShare = 0.049 },
                    new Tag { Id = 5, Name = "php", Count = 1464460, PercentageShare = 0.0444 },
                    new Tag { Id = 6, Name = "android", Count = 1417220, PercentageShare = 0.043 },
                    new Tag { Id = 7, Name = "html", Count = 1187321, PercentageShare = 0.036 },
                    new Tag { Id = 8, Name = "jquery", Count = 1034792, PercentageShare = 0.0314 },
                    new Tag { Id = 9, Name = "c++", Count = 806767, PercentageShare = 0.0245 },
                    new Tag { Id = 10, Name = "css", Count = 804224, PercentageShare = 0.0244 },
                    new Tag { Id = 11, Name = "ios", Count = 687251, PercentageShare = 0.0208 },
                    new Tag { Id = 12, Name = "sql", Count = 670784, PercentageShare = 0.0203 },
                    new Tag { Id = 13, Name = "mysql", Count = 662008, PercentageShare = 0.0201 },
                    new Tag { Id = 14, Name = "r", Count = 505627, PercentageShare = 0.0153 },
                    new Tag { Id = 15, Name = "reactjs", Count = 476692, PercentageShare = 0.0145 },
                    new Tag { Id = 16, Name = "node.js", Count = 472005, PercentageShare = 0.0143 },
                    new Tag { Id = 17, Name = "arrays", Count = 416706, PercentageShare = 0.0126 },
                    new Tag { Id = 18, Name = "c", Count = 403965, PercentageShare = 0.0123 },
                    new Tag { Id = 19, Name = "asp.net", Count = 374611, PercentageShare = 0.0114 },
                    new Tag { Id = 20, Name = "json", Count = 360340, PercentageShare = 0.0109 }
                },
                5,
                3,
                "name",
                "asc",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>()
                    {
                            new Tag { Id = 1, Name = "javascript", Count = 2528869, PercentageShare = 0.0767 },
                            new Tag { Id = 8, Name = "jquery", Count = 1034792, PercentageShare = 0.0314 },
                            new Tag { Id = 20, Name = "json", Count = 360340, PercentageShare = 0.0109 },
                            new Tag { Id = 13, Name = "mysql", Count = 662008, PercentageShare = 0.0201 },
                            new Tag { Id = 16, Name = "node.js", Count = 472005, PercentageShare = 0.0143 }
                    },
                    ItemsFrom = 11,
                    ItemsTo = 15,
                    TotalItems = 20,
                    TotalPages = 4,
                }
            },
            new object[]
            {
                new List<Tag>()
                {
                    new Tag { Id = 1, Name = "javascript", Count = 2528869, PercentageShare = 0.0767 },
                    new Tag { Id = 2, Name = "python", Count = 2192349, PercentageShare = 0.0665 },
                    new Tag { Id = 3, Name = "java", Count = 1917308, PercentageShare = 0.0582 },
                    new Tag { Id = 4, Name = "c#", Count = 1615064, PercentageShare = 0.049 },
                    new Tag { Id = 5, Name = "php", Count = 1464460, PercentageShare = 0.0444 },
                    new Tag { Id = 6, Name = "android", Count = 1417220, PercentageShare = 0.043 },
                    new Tag { Id = 7, Name = "html", Count = 1187321, PercentageShare = 0.036 },
                    new Tag { Id = 8, Name = "jquery", Count = 1034792, PercentageShare = 0.0314 },
                    new Tag { Id = 9, Name = "c++", Count = 806767, PercentageShare = 0.0245 },
                    new Tag { Id = 10, Name = "css", Count = 804224, PercentageShare = 0.0244 },
                    new Tag { Id = 11, Name = "ios", Count = 687251, PercentageShare = 0.0208 },
                    new Tag { Id = 12, Name = "sql", Count = 670784, PercentageShare = 0.0203 },
                    new Tag { Id = 13, Name = "mysql", Count = 662008, PercentageShare = 0.0201 },
                    new Tag { Id = 14, Name = "r", Count = 505627, PercentageShare = 0.0153 },
                    new Tag { Id = 15, Name = "reactjs", Count = 476692, PercentageShare = 0.0145 },
                    new Tag { Id = 16, Name = "node.js", Count = 472005, PercentageShare = 0.0143 },
                    new Tag { Id = 17, Name = "arrays", Count = 416706, PercentageShare = 0.0126 },
                    new Tag { Id = 18, Name = "c", Count = 403965, PercentageShare = 0.0123 },
                    new Tag { Id = 19, Name = "asp.net", Count = 374611, PercentageShare = 0.0114 },
                    new Tag { Id = 20, Name = "json", Count = 360340, PercentageShare = 0.0109 }
                },
                12,
                2,
                "count",
                "desc",
                new PagedResult<Tag>()
                {
                    Items = new List<Tag>()
                    {
                            new Tag { Id = 13, Name = "mysql", Count = 662008, PercentageShare = 0.0201 },
                            new Tag { Id = 14, Name = "r", Count = 505627, PercentageShare = 0.0153 },
                            new Tag { Id = 15, Name = "reactjs", Count = 476692, PercentageShare = 0.0145 },
                            new Tag { Id = 16, Name = "node.js", Count = 472005, PercentageShare = 0.0143 },
                            new Tag { Id = 17, Name = "arrays", Count = 416706, PercentageShare = 0.0126 },
                            new Tag { Id = 18, Name = "c", Count = 403965, PercentageShare = 0.0123 },
                            new Tag { Id = 19, Name = "asp.net", Count = 374611, PercentageShare = 0.0114 },
                            new Tag { Id = 20, Name = "json", Count = 360340, PercentageShare = 0.0109 }
                    },
                    ItemsFrom = 13,
                    ItemsTo = 24,
                    TotalItems = 20,
                    TotalPages = 2,
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
            await using var context = TestApplicationDbContextInMemoryCreator.Create();

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
            await using var context = TestApplicationDbContextInMemoryCreator.Create();

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
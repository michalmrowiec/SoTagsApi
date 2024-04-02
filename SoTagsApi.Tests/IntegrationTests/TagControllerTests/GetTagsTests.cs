using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SoTagsApi.Application.Functions.Tags.Queries;
using SoTagsApi.Domain.Models;
using SoTagsApi.Infrastructure;

namespace SoTagsApi.Tests.IntegrationTests.TagControllerTests
{
    public class GetTagsTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public GetTagsTests(
            CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        public static IEnumerable<object[]> ValidDataForGetTags => new List<object[]>
        {
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new Tag { Id = 2, Name = "js", Count = 20, PercentageShare = 0.2 },
                        new Tag { Id = 3, Name = "python", Count = 10, PercentageShare = 0.1 }
                },
                10,
                1,
                "percentageshare",
                "desc",
                new PagedResult<TagDto>()
                {
                    Items = new List<TagDto>()
                    {
                        new TagDto { Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new TagDto { Name = "js", Count = 20, PercentageShare = 0.2 },
                        new TagDto { Name = "python", Count = 10, PercentageShare = 0.1 }
                    },
                    ItemsFrom = 1,
                    ItemsTo = 10,
                    TotalItems = 3,
                    TotalPages = 1,
                }
            },
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new Tag { Id = 2, Name = "js", Count = 20, PercentageShare = 0.2 },
                        new Tag { Id = 3, Name = "python", Count = 10, PercentageShare = 0.1 }
                },
                10,
                1,
                "count",
                "asc",
                new PagedResult<TagDto>()
                {
                    Items = new List<TagDto>()
                    {
                        new TagDto { Name = "python", Count = 10, PercentageShare = 0.1 },
                        new TagDto { Name = "js", Count = 20, PercentageShare = 0.2 },
                        new TagDto { Name = "c#", Count = 70, PercentageShare = 0.7 }
                    },
                    ItemsFrom = 1,
                    ItemsTo = 10,
                    TotalItems = 3,
                    TotalPages = 1,
                }
            },
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new Tag { Id = 2, Name = "js", Count = 20, PercentageShare = 0.2 },
                        new Tag { Id = 3, Name = "python", Count = 10, PercentageShare = 0.1 }
                },
                100,
                10_000,
                "count",
                "asc",
                new PagedResult<TagDto>()
                {
                    Items = new List<TagDto>(),
                    ItemsFrom = 999_901,
                    ItemsTo = 10_00_000,
                    TotalItems = 3,
                    TotalPages = 1,
                }
            }
        };


        [Theory]
        [MemberData(nameof(ValidDataForGetTags))]
        public async Task GetTags_ForValidData_ReturnsPaginedSortedTags(
            List<Tag> tags,
            int pageSize,
            int pageNumber,
            string sortProperty,
            string sortOrder,
            PagedResult<TagDto> expectedResponse)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.RemoveRange(dbContext.Tags);
            await dbContext.Tags.AddRangeAsync(tags);
            await dbContext.SaveChangesAsync();

            var uri = $"/Tags?pageSize={pageSize}&pageNumber={pageNumber}&sortProperty={sortProperty}&sortOrder={sortOrder}";
            var response = await _client.GetAsync(uri);
            var responseString = await response.Content.ReadAsStringAsync();
            var pagedResult = JsonConvert.DeserializeObject<PagedResult<TagDto>>(responseString);


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            pagedResult.Should().BeEquivalentTo(expectedResponse);
        }

        public static IEnumerable<object[]> InvalidDataForGetTags => new List<object[]>
        {
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new Tag { Id = 2, Name = "js", Count = 20, PercentageShare = 0.2 },
                        new Tag { Id = 3, Name = "python", Count = 10, PercentageShare = 0.1 }
                },
                0,
                1,
                "percentageshare",
                "desc"
            },
            new object[]
            {
                new List<Tag>()
                {
                        new Tag { Id = 1, Name = "c#", Count = 70, PercentageShare = 0.7 },
                        new Tag { Id = 2, Name = "js", Count = 20, PercentageShare = 0.2 },
                        new Tag { Id = 3, Name = "python", Count = 10, PercentageShare = 0.1 }
                },
                -1,
                1,
                "",
                "asc"
            },
            new object[]
            {
                new List<Tag>(),
                10,
                1,
                "sadfdaf",
                "asc"
            },
            new object[]
            {
                new List<Tag>(),
                10,
                0,
                "sadfdaf",
                "asc"
            },
            new object[]
            {
                new List<Tag>(),
                101,
                1,
                "name",
                "asc"
            },
            new object[]
            {
                new List<Tag>(),
                10,
                10_001,
                "name",
                "asc"
            },
            new object[]
            {
                new List<Tag>(),
                0,
                1,
                "",
                ""
            },
            new object[]
            {
                new List<Tag>(),
                5,
                0,
                "",
                ""
            }
        };

        [Theory]
        [MemberData(nameof(InvalidDataForGetTags))]
        public async Task GetTags_ForInvalidData_ReturnsBadRequestStatus(
            List<Tag> tags,
            int pageSize,
            int pageNumber,
            string sortProperty,
            string sortOrder)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.RemoveRange(dbContext.Tags);
            await dbContext.Tags.AddRangeAsync(tags);
            await dbContext.SaveChangesAsync();

            var uri = $"/Tags?pageSize={pageSize}&pageNumber={pageNumber}&sortProperty={sortProperty}&sortOrder={sortOrder}";
            var response = await _client.GetAsync(uri);
            var responseString = await response.Content.ReadAsStringAsync();


            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}

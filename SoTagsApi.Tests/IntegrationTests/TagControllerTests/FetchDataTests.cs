using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoTagsApi.Infrastructure;

namespace SoTagsApi.Tests.IntegrationTests.TagControllerTests
{
    public class FetchDataTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public FetchDataTests(
            CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(10, 100)]
        [InlineData(50, 100)]
        [InlineData(300, 300)]
        [InlineData(2500, 2500)]
        public async Task FetchTags_ForValidData_ReturnsOkStatus(int count, int expectedCount)
        {
            var response = await _client.PostAsync($"/Tags/{count}", null);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tags = await dbContext.Tags.ToListAsync();
            tags.Should().HaveCount(expectedCount);
        }

        [Theory]
        [InlineData(-99, 0)]
        [InlineData(-1, 0)]
        [InlineData(0, 0)]
        [InlineData(1, 0)]
        [InlineData(9, 0)]
        [InlineData(2501, 0)]
        public async Task FetchTags_ForInvalidData_ReturnsBadRequestStatus(int count, int expectedCount)
        {
            var response = await _client.PostAsync($"/Tags/{count}", null);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tags = await dbContext.Tags.ToListAsync();
            tags.Should().HaveCount(expectedCount);
        }
    }
}

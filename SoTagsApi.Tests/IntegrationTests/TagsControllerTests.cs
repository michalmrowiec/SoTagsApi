using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using SoTagsApi.Application.Functions.Tags.Commands.FetchTags;
using SoTagsApi.Infrastructure;
using SoTagsApi.Tests.UnitTests.Infrastructure.Helper;
using System.Text;
using Xunit.Sdk;

namespace SoTagsApi.Tests.IntegrationTests
{
    public class TagsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public TagsControllerTests(
            CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("")]
        [InlineData("10")]
        [InlineData("50")]
        [InlineData("2500")]
        public async Task FetchTags_ForValidData_ReturnsOkStatus(string count)
        {
            var uri = $"/Tags/{count}";
            var response = await _client.PostAsync($"/Tags/{count}", null);
            _ = await response.Content.ReadAsStringAsync();

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tags = await dbContext.Tags.ToListAsync();
            tags.Should().NotBeEmpty();
        }
    }
}

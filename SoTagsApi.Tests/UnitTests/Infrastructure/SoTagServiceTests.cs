using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using Newtonsoft.Json;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Infrastructure.Services;
using System.Net;
using FluentAssertions;
using System.IO.Compression;
using System.Text;

namespace SoTagsApi.Tests.UnitTests.Infrastructure
{
    public class SoTagServiceTests
    {
        [Fact]
        public async Task FetchTagsAsync_ForValidData_ReturnsTrue()
        {

        }

    }
}

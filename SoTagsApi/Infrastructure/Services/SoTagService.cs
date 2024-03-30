using Newtonsoft.Json;
using SoTagsApi.Domain.Interfaces;
using System.IO.Compression;
using System.Net;

namespace SoTagsApi.Infrastructure.Services
{
    public class SoTagService : ISoTagService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly ILogger<SoTagService> _logger;
        public SoTagService(ITagsRepository tagsRepository, ILogger<SoTagService> logger)
        {
            _tagsRepository = tagsRepository;
            _logger = logger;
        }

        public async Task FetchTagsAsync(int tagsToFetch = 1000)
        {
            if (tagsToFetch < 0 || tagsToFetch > 10_000)
            {
                //log
                return;
            }

            var temp1 = tagsToFetch / 100;
            if (tagsToFetch % 100 != 0)
                temp1++;
            tagsToFetch = temp1 * 100;

            var tags = CalcPercentageShare(
                await FetchTagsFromApiAsync(tagsToFetch));

            foreach (var tag in tags)
            {
                await Console.Out.WriteLineAsync($"{tag.Name,-20} {tag.Count,-10} {tag.PercentageShare,-5}");
            }
            await Console.Out.WriteLineAsync($"Sum: {tags.Sum(t => t.PercentageShare)} Count: {tags.Count}");

            await _tagsRepository.DeleteAllAsync();
            await _tagsRepository.CreateRangeAsync(tags);

            return;
        }
        private async Task<IList<Tag>> FetchTagsFromApiAsync(int tagsToFetch = 1000)
        {
            int pageNumber = 1;
            int pageSize = 100;
            List<Tag> tags = new();

            HttpClient client = new();
            UriBuilder uriBuilder = new("https://api.stackexchange.com/2.3/tags");

            while (pageNumber * pageSize < tagsToFetch + 1)
            {
                uriBuilder.Query = $"page={pageNumber}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow";

                client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");

                var response = await client.GetAsync(uriBuilder.Uri);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    return new List<Tag>();
                }

                var content = new StreamReader(
                    new GZipStream(await response.Content.ReadAsStreamAsync(),
                    CompressionMode.Decompress))
                    .ReadToEnd();

                var model = JsonConvert.DeserializeObject<ResponseModel>(content);

                if (model?.Items == null)
                {
                    break;
                }

                tags.AddRange(model.Items);

                pageNumber++;
            }

            return tags;
        }

        private static IList<Tag> CalcPercentageShare(IList<Tag> tags)
        {
            var totalCount = tags.Sum(x => x.Count);

            foreach (var tag in tags)
            {
                tag.PercentageShare = Math.Round((double)tag.Count / totalCount, 4);
            }

            return tags;
        }
    }
}

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
        private readonly HttpClient _httpClient;
        public SoTagService(ITagsRepository tagsRepository, ILogger<SoTagService> logger, HttpClient httpClient)
        {
            _tagsRepository = tagsRepository;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<bool> FetchTagsAsync(int tagsToFetch = 1000)
        {
            if (tagsToFetch < 0 || tagsToFetch > 2500)
            {
                _logger.LogWarning($"TagsToFeth is invalid (less than 0 or grether than 10000) has value: {tagsToFetch}");
                return false;
            }

            var temp1 = tagsToFetch / 100;
            if (tagsToFetch % 100 != 0)
                temp1++;
            tagsToFetch = temp1 * 100;

            _logger.LogInformation($"Starting fetch {tagsToFetch} new tags");
            var tags = CalcPercentageShare(
                await FetchTagsFromApiAsync(tagsToFetch));

            if (tags.Count <= 0)
            {
                _logger.LogError($"Fetched tags count: {tags.Count}");
                return false;
            }

            await _tagsRepository.DeleteAllAsync();
            _logger.LogInformation("Deleted tags from db");
            await _tagsRepository.CreateRangeAsync(tags);
            _logger.LogInformation("Saved new tags in db");

            return true;
        }
        private async Task<IList<Tag>> FetchTagsFromApiAsync(int tagsToFetch = 1000)
        {
            int pageNumber = 1;
            int pageSize = 100;
            List<Tag> tags = [];

            UriBuilder uriBuilder = new("https://api.stackexchange.com/2.3/tags");

            try
            {
                while (pageNumber * pageSize < tagsToFetch + 1)
                {
                    uriBuilder.Query = $"page={pageNumber}&pagesize={pageSize}&order=desc&sort=popular&site=stackoverflow";
                    _logger.LogInformation($"Sending request to {uriBuilder.Uri}");

                    _httpClient.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");

                    var response = await _httpClient.GetAsync(uriBuilder.Uri);
                    _logger.LogInformation($"Received response with status code {response.StatusCode}");

                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError($"{response.StatusCode}|{response}|{response.Headers}|{response.Content}||PageNumber:{pageNumber}|PageSize:{pageSize}");

                        return [];
                    }

                    var content = new StreamReader(
                        new GZipStream(await response.Content.ReadAsStreamAsync(),
                        CompressionMode.Decompress))
                        .ReadToEnd();

                    _logger.LogInformation($"Response content: {content}");
                    var model = JsonConvert.DeserializeObject<ResponseModel>(content);
                    _logger.LogInformation($"Deserialized data: {model}");

                    if (model?.Items == null)
                    {
                        _logger.LogWarning($"Response items is null: {model}");

                        break;
                    }

                    tags.AddRange(model.Items);

                    pageNumber++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
            }

            return tags;
        }

        private IList<Tag> CalcPercentageShare(IList<Tag> tags)
        {
            _logger.LogInformation($"Start calc percentage share of {tags.Count} tags");
            var totalCount = tags.Sum(x => x.Count);

            foreach (var tag in tags)
            {
                tag.PercentageShare = Math.Round((double)tag.Count / totalCount, 4);
            }

            return tags;
        }
    }
}

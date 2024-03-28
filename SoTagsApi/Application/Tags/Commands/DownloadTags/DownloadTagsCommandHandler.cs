using MediatR;
using Newtonsoft.Json;
using SoTagsApi.Application.Tags.Queries;
using System.IO.Compression;
using System.Net;

namespace SoTagsApi.Application.Tags.Commands.DownloadTags
{
    public class DownloadTagsCommandHandler : IRequestHandler<DownloadTagsCommand, bool>
    {
        public async Task<bool> Handle(DownloadTagsCommand request, CancellationToken cancellationToken)
        {
            HttpClient client = new HttpClient();

            UriBuilder uriBuilder = new("https://api.stackexchange.com/2.3/tags")
            {
                Query = "page=1&pagesize=20&order=desc&sort=popular&site=stackoverflow"
            };

            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
            
            var response = await client.GetAsync(uriBuilder.Uri);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var content = new StreamReader(
                new GZipStream(await response.Content.ReadAsStreamAsync(),
                CompressionMode.Decompress))
                .ReadToEnd();

            var model = JsonConvert.DeserializeObject<ResponseModel>(content);

            List<Tag> tags = model?.Items ?? new();

            var totalCount = tags.Sum(x => x.Count);

            List<TagDto> tagsDto = new();

            foreach (var tag in tags)
            {
                var percentageShare = Math.Round((float)tag.Count / totalCount, 2);

                tagsDto.Add(new TagDto(tag.Name, tag.Count, (float)percentageShare));
            }
            foreach (var tagDto in tagsDto)
            {
                await Console.Out.WriteLineAsync($"{tagDto.Name,-20} {tagDto.Count,-10} {tagDto.PercentageShare,-5}");
            }
            await Console.Out.WriteLineAsync($"Sum: {tagsDto.Sum(t => t.PercentageShare)}");
            Console.WriteLine("Downloaded");

            return true;
        }
    }
}

public class ResponseModel
{
    public List<Tag> Items { get; set; }
    public bool HasMore { get; set; }
    public int QuotaMax { get; set; }
    public int QuotaRemaining { get; set; }
}

public class Tag
{
    public string Name { get; set; }
    public long Count { get; set; }
}
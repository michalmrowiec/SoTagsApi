using MediatR;

namespace SoTagsApi.Application.Tags.Commands.DownloadTags
{
    public record FetchTagsCommand(int Count) : IRequest<bool>;
}

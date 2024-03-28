using MediatR;

namespace SoTagsApi.Application.Tags.Commands.DownloadTags
{
    public record DownloadTagsCommand(int Count) : IRequest<bool>;
}

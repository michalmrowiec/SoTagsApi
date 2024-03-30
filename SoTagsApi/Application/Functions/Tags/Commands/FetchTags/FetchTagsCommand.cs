using MediatR;

namespace SoTagsApi.Application.Functions.Tags.Commands.FetchTags
{
    public record FetchTagsCommand(int Count) : IRequest<bool>;
}

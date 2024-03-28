using MediatR;
using SoTagsApi.Domain.Models;

namespace SoTagsApi.Application.Tags.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, PagedResult<TagDto>>
    {
        public Task<PagedResult<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

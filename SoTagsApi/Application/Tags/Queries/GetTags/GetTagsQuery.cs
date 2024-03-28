using MediatR;
using SoTagsApi.Domain.Models;

namespace SoTagsApi.Application.Tags.Queries.GetTags
{
    public class GetTagsQuery : IRequest<PagedResult<TagDto>>
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string SortOptions { get; set; } = string.Empty;
    }
}

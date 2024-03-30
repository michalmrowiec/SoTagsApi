using MediatR;
using SoTagsApi.Domain.Models;

namespace SoTagsApi.Application.Tags.Queries.GetTags
{
    public class GetTagsQuery : IRequest<PagedResult<TagDto>>
    {
        public GetTagsQuery(int pageSize, int pageNumber, string? sortProperty, string? sortOrder)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            SortProperty = sortProperty;
            SortOrder = sortOrder;
        }

        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SortProperty { get; set; }
        public string? SortOrder { get; set; }
    }
}

using AutoMapper;
using MediatR;
using SoTagsApi.Application.Tags.Commands.DownloadTags;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Domain.Models;

namespace SoTagsApi.Application.Tags.Queries.GetTags
{
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, PagedResult<TagDto>>
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly ILogger<FetchTagsCommandHandler> _logger;
        private readonly IMapper _mapper;

        public GetTagsQueryHandler(
            ITagsRepository tagsRepository,
            ILogger<FetchTagsCommandHandler> logger,
            IMapper mapper)
        {
            _tagsRepository = tagsRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PagedResult<TagDto>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var pagedResult = await _tagsRepository.GetPaginedSortedTags(
                request.PageSize != 0 ? request.PageSize : 10,
                request.PageNumber != 0 ? request.PageNumber : 1,
                request.SortProperty ?? "name",
                request.SortOrder ?? "desc");

            var pagedResultDto = _mapper.Map<PagedResult<TagDto>>(pagedResult);

            return pagedResultDto;
        }
    }
}

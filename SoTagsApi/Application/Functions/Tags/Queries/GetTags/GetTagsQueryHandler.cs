using AutoMapper;
using MediatR;
using SoTagsApi.Application.Functions.Tags.Commands.FetchTags;
using SoTagsApi.Domain.Interfaces;
using SoTagsApi.Domain.Models;
using System.Text.RegularExpressions;

namespace SoTagsApi.Application.Functions.Tags.Queries.GetTags
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
            if (!Regex.IsMatch(request.SortProperty, "^(name|count|percentageshare|)$")
                || !Regex.IsMatch(request.SortOrder, "^(asc|desc|)$")
                || request.PageSize <= 0
                || request.PageSize > 100
                || request.PageNumber <= 0
                || request.PageSize > 10_000)
            {
                _logger.LogError($"Invalid parameters | sort property: {request.SortProperty} | sort order: {request.SortOrder} | page size: {request.PageSize} | page number: {request.PageNumber}");
                return new();
            }

            try
            {
                _logger.LogInformation($"Handling GetTagsQuery: {request}");

                var pagedResult = await _tagsRepository.GetPaginedSortedTags(
                    request.SortProperty,
                    request.SortOrder,
                    request.PageSize,
                    request.PageNumber);

                _logger.LogInformation($"Fetched {pagedResult.Items.Count} tags from repository");

                var pagedResultDto = _mapper.Map<PagedResult<TagDto>>(pagedResult);

                _logger.LogInformation($"Mapped fetched tags to TagDto");

                return pagedResultDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while handling GetTagsQuery: {request}");
            }

            return new PagedResult<TagDto>();
        }
    }
}

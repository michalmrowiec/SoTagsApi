using MediatR;
using SoTagsApi.Domain.Interfaces;

namespace SoTagsApi.Application.Functions.Tags.Commands.FetchTags
{
    public class FetchTagsCommandHandler : IRequestHandler<FetchTagsCommand, bool>
    {
        private readonly ISoTagService _soTagService;
        private readonly ILogger<FetchTagsCommandHandler> _logger;

        public FetchTagsCommandHandler(ISoTagService soTagService, ILogger<FetchTagsCommandHandler> logger)
        {
            _soTagService = soTagService;
            _logger = logger;
        }

        public async Task<bool> Handle(FetchTagsCommand request, CancellationToken cancellationToken)
        {
            if (request.Count < 0 || request.Count > 2500)
            {
                _logger.LogWarning($"Tags to feth is invalid (less than 0 or grether than 10000) has value: {request.Count}");
                return false;
            }

            var result = await _soTagService.FetchTagsAsync(request.Count);

            return result;
        }
    }
}

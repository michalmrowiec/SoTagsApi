using MediatR;
using SoTagsApi.Domain.Interfaces;

namespace SoTagsApi.Application.Tags.Commands.DownloadTags
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
            await _soTagService.FetchTagsAsync(request.Count);

            Console.WriteLine("Downloaded");

            return true;
        }
    }
}

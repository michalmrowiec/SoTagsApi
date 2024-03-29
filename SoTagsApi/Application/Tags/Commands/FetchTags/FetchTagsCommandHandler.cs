using MediatR;
using SoTagsApi.Domain.Interfaces;

namespace SoTagsApi.Application.Tags.Commands.DownloadTags
{
    public class FetchTagsCommandHandler : IRequestHandler<FetchTagsCommand, bool>
    {
        private readonly ISoTagService _soTagService;

        public FetchTagsCommandHandler(ISoTagService soTagService)
        {
            _soTagService = soTagService;
        }

        public async Task<bool> Handle(FetchTagsCommand request, CancellationToken cancellationToken)
        {
            await _soTagService.FetchTagsAsync(request.Count);

            Console.WriteLine("Downloaded");

            return true;
        }
    }
}

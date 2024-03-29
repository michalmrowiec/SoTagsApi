using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoTagsApi.Application.Tags.Commands.DownloadTags;

namespace SoTagsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{count?}")]
        public async Task<IActionResult> DownloadTags([FromRoute] int count = 1000)
        {
            await _mediator.Send(new FetchTagsCommand(count));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            return Ok();
        }
    }
}

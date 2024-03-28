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

        [HttpPost]
        public async Task<IActionResult> DownloadTags()
        {
            await _mediator.Send(new DownloadTagsCommand(20));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTags()
        {
            return Ok();
        }
    }
}

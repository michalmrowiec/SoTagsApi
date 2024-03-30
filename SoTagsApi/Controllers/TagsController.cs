using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoTagsApi.Application.Tags.Commands.DownloadTags;
using SoTagsApi.Application.Tags.Queries.GetTags;

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
        public async Task<IActionResult> GetTags(
            [FromQuery] int pageSize,
            [FromQuery] int pageNumber,
            [FromQuery] string? sortProperty,
            [FromQuery] string? sortOrder)
        {
            var response = await _mediator.Send(
                new GetTagsQuery(pageSize, pageNumber, sortProperty, sortOrder));

            return Ok(response);
        }
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using SoTagsApi.Application.Functions.Tags.Commands.FetchTags;
using SoTagsApi.Application.Functions.Tags.Queries;
using SoTagsApi.Application.Functions.Tags.Queries.GetTags;
using SoTagsApi.Domain.Models;
using System.ComponentModel.DataAnnotations;

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
            _mediator.Send(new FetchTagsCommand(2000));
        }

        [HttpPost("{count?}")]
        public async Task<IActionResult> FetchTags(
            [FromRoute, Range(10, 2500, ErrorMessage = "Count must be between 1 and 2500")] int count = 1000)
        {
            var success = await _mediator.Send(new FetchTagsCommand(count));

            return success ? Ok() : BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TagDto>>> GetTags(
            [FromQuery, Required, Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")] int pageSize,
            [FromQuery, Required, Range(1, 10_000, ErrorMessage = "Page number must be greater than 0")] int pageNumber,
            [FromQuery, RegularExpression("^(name|count|percentageshare|)$", ErrorMessage = "Invalid sort property. Valid options are: name, count, percentageshare")] string? sortProperty,
            [FromQuery, RegularExpression("^(asc|desc|)$", ErrorMessage = "Invalid sort order. Valid options are: asc, desc")] string? sortOrder)
        {
            sortProperty ??= "percentageshare";
            sortOrder ??= "desc";

            var response = await _mediator.Send(
                new GetTagsQuery(pageSize, pageNumber, sortProperty, sortOrder));

            if (response.TotalItems == 0)
            {
                await FetchTags();
                response = await _mediator.Send(
                new GetTagsQuery(pageSize, pageNumber, sortProperty, sortOrder));
            }

            return Ok(response);
        }
    }
}

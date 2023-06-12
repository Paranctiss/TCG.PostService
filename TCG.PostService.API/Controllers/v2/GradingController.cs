using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.Gradings.Query;

namespace TCG.PostService.API.Controllers.v2;

[ApiController]
[Route("[controller]")]
[ApiVersion("2.0")]
public class GradingController : ControllerBase
{
    private readonly IMediator _mediator;

    public GradingController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetGrading(CancellationToken cancellationToken)
    {
        var grading = await _mediator.Send(new GetGradingQuery(), cancellationToken);

        if (grading == null)
        {
            return NotFound();
        }
        return Ok(grading);
    }
}

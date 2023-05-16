using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.Gradings.Query;
using TCG.PostService.Application.SalePost.Command;
using TCG.PostService.Application.SalePost.DTO.Request;
using TCG.PostService.Application.SalePost.Query;

namespace TCG.PostService.API.Controllers;

[ApiController]
[Route("[controller]")]
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

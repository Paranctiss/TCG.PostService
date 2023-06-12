using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.SalePost.Command;
using TCG.PostService.Application.SalePost.DTO.Request;
using TCG.PostService.Application.SalePost.Query;

namespace TCG.PostService.API.Controllers.v1;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class SalePostController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalePostController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostSalePost([FromBody] SalePostDtoRequest salePostDtoRequest,
        CancellationToken cancellationToken)

    {
        var salePost = await _mediator.Send(new CreateSalePostCommand(salePostDtoRequest), cancellationToken);
        return CreatedAtAction(nameof(GetSalePost), new { id = salePost.Id }, salePost);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalePost(Guid id, CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new GetSalePostQuery(id), cancellationToken);

        return salePost != null ? Ok(salePost) : NotFound();
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetSalePostPublic(string idReference, string idExtensions, string idGradings, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        string[] idExtensionsArray = idExtensions.Split(",");
        string[] idGradingsArray = idGradings.Split(",");
        var salePost = await _mediator.Send(new GetSalePostPublicQuery(idReference, idExtensionsArray, idGradingsArray, pageNumber, pageSize), cancellationToken);

        if (salePost == null)
        {
            return NotFound();
        }
        return Ok(salePost);
    }

    [HttpGet("user/{userId}/{pageSize}")]
    public async Task<IActionResult> GetLastUserSalePost(int pageSize, int userId, CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new GetUserSalePostQuery(pageSize, userId), cancellationToken);

        if (salePost == null)
        {
            return NotFound();
        }
        return Ok(salePost);
    }
}
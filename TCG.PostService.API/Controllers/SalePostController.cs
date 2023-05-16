using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.Command;
using TCG.PostService.Application.SalePost.DTO.Request;
using TCG.PostService.Application.SalePost.Query;
using TCG.PostService.Domain;

namespace TCG.PostService.API.Controllers;

[ApiController]
[Route("[controller]")]
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
    public async Task<IActionResult> GetSalePost(int id, CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new GetSalePostQuery(id), cancellationToken);

        return salePost != null ? Ok(salePost) : NotFound();
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetSalePostPublic(CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new GetSalePostPublicQuery(), cancellationToken);

        if (salePost == null)
        {
            return NotFound();
        }
        return Ok(salePost);
    }
}
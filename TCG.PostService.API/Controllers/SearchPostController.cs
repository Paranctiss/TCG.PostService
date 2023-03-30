using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.Query;
using TCG.PostService.Domain;

namespace TCG.PostService.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchPostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    
    public SearchPostController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> CreateOfferPost([FromBody] SearchPostDto searchPostDto, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CreateSearchPostCommand(searchPostDto);
            var result = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetSearchPost), new { id = result.Id }, result);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSearchPost(int id, CancellationToken cancellationToken)
    {
        var searchPost = await _mediator.Send(new GetSearchPostQuery(id), cancellationToken);

        if (searchPost == null)
        {
            return NotFound();
        }
        return Ok(searchPost);
    }
}
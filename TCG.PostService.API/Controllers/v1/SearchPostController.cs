using Asp.Versioning;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.Query;

namespace TCG.PostService.API.Controllers.v1;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
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
    public async Task<IActionResult> CreateOfferPost([FromBody] SearchPostDtoRequest searchPostDto, CancellationToken cancellationToken)
    {
        var command = new CreateSearchPostCommand(searchPostDto);
        var result = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetSearchPost), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSearchPost(Guid id, CancellationToken cancellationToken)
    {
        var searchPost = await _mediator.Send(new GetSearchPostQuery(id), cancellationToken);

        if (searchPost == null)
        {
            return NotFound();
        }
        return Ok(searchPost);
    }

    [HttpGet("public")]
    public async Task<IActionResult> GetSearchPostPublic(string idReference, string idExtensions, string idGradings, string idUser, CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        string[] idExtensionsArray = idExtensions.Split(",");
        string[] idGradingsArray = idGradings.Split(",");
        var searchPost = await _mediator.Send(new GetSearchPostPublicQuery(idReference, idExtensionsArray, idGradingsArray, idUser, pageNumber, pageSize), cancellationToken);

        if (searchPost == null)
        {
            return NotFound();
        }
        return Ok(searchPost);
    }

    [HttpGet("user/{id}/{nbMax}")]
    public async Task<IActionResult> GetLastUserSearchPost(int id, int nbMax, CancellationToken cancellationToken)
    {
        var searchPost = await _mediator.Send(new GetUserSearchPostQuery(id, nbMax), cancellationToken);

        if (searchPost == null)
        {
            return NotFound();
        }
        return Ok(searchPost);
    }

    [HttpPut("public/{id}")]
    public async Task<IActionResult> UpdateIsPublic(Guid id, int idUser, CancellationToken cancellationToken)
    {
        var searchPost = await _mediator.Send(new UpdateSearchPostCommand(id, idUser), cancellationToken);

        if (searchPost == null)
        {
            return NotFound();
        }
        return Ok(searchPost);
    }

}
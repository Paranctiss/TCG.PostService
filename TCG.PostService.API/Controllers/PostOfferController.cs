using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.Offer.Command;
using TCG.PostService.Application.Offer.DTO;
using TCG.PostService.Domain;

namespace TCG.PostService.API.Controllers;

[ApiController]
public class PostOfferController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public PostOfferController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }
    
    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateOfferPost([FromBody] OfferPostDto offerPostDto)
    {
        var offerPost = _mapper.Map<OfferPost>(offerPostDto);
        var command = new CreateOfferCommand(offerPost);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
using Asp.Versioning;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TCG.PostService.Application.Offer.Command;
using TCG.PostService.Application.Offer.DTO;

namespace TCG.PostService.API.Controllers.v1;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfferController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add")]
    public async Task<IActionResult> PostOffer([FromBody]OfferDtoRequest offerDto, CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new CreateOfferCommand(offerDto), cancellationToken);
        return CreatedAtAction("PostOffer", "");
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateOffer(int offerId, char offerStateId, CancellationToken cancellationToken)
    {
        var salePost = await _mediator.Send(new UpdateOfferCommand(offerId, offerStateId), cancellationToken);
        return CreatedAtAction("UpdateOffer", "");
    }
}
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Offer.Command;

public record CreateOfferCommand(OfferPost OfferPost) : IRequest<OfferPost>;

public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, OfferPost>
{
    private readonly ILogger<CreateOfferCommandHandler> _logger;
    private readonly IOfferPostRepository _repository;

    public CreateOfferCommandHandler(ILogger<CreateOfferCommandHandler> logger, IOfferPostRepository dbService)
    {
        _logger = logger;
        _repository = dbService;
    }

    public async Task<OfferPost> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.AddAsync(request.OfferPost, cancellationToken);
            return request.OfferPost;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while adding pokemon item");
            throw;
        }
    }
}
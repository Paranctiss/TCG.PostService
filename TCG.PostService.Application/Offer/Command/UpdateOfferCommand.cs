using FluentValidation;
using MapsterMapper;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.Extensions.Logging;
using Mysqlx.Crud;
using System.Text.Json;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.Offer.DTO;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.Offer.Command
{
    public record UpdateOfferCommand(int offerId, char offerStateId) : IRequest;

    public class UpdateOfferValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferValidator()
        {
            RuleFor(sp => sp.offerId).NotEmpty();
            RuleFor(sp => sp.offerStateId).NotEmpty();
        }
    }

    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand>
    {
        private readonly ISalePostRepository _repositorySalePost;
        private readonly IOfferRepository _repositoryOffer;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<UserById> _requestClient;
        private readonly ILogger<UpdateOfferCommandHandler> _logger;

        public UpdateOfferCommandHandler(IOfferRepository repositoryOffer, ISalePostRepository repositorySalePost, IMapper mapper, IPublishEndpoint publishEndpoint, IRequestClient<UserById> requestClient, ILogger<UpdateOfferCommandHandler> logger)
        {
            _repositorySalePost = repositorySalePost;
            _repositoryOffer = repositoryOffer;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateOfferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var offer = await _repositoryOffer.GetOfferById(request.offerId, cancellationToken);

                await _repositoryOffer.ExecuteInTransactionAsync(async () =>
                {
                    if (offer != null)
                    {
                        offer.OfferStatePostId = request.offerStateId;
                        await _repositoryOffer.UpdateOffer(offer, cancellationToken);
                    }
                    offer = await _repositoryOffer.GetOfferById(request.offerId, cancellationToken);
                    if (offer!=null && offer.OfferStatePostId == request.offerStateId)
                    {
                        var offerMessageUpdate = new UpdateOfferInMessage(JsonSerializer.Serialize(offer));
                        await _publishEndpoint.Publish(offerMessageUpdate, cancellationToken);
                    }

                }, cancellationToken);
                
                if (offer != null)
                {
                    _logger.LogInformation("Offer updated with id {Id}", offer.Id);
                }
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError("Error during update of an offer with error {error}", e.Message);
                throw;
            }
        }
    }
}
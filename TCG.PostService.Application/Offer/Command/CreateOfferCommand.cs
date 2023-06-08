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
    public class message
    {
        public offre offre { get; set; }
        public int idUserEnvoi { get; set; }
        public DateTime dateEnvoi { get; set; }
        public string texte { get; set; }
    }

    public class offre
    {
        public int id { get; set; }
        public decimal prixPropose { get; set; }
        public string etat { get; set; }
    }

    public class user
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string photoProfil { get; set; }
    }

    public record CreateOfferCommand(OfferDtoRequest OfferDtoRequest) : IRequest;

    public class AddOfferValidator : AbstractValidator<CreateOfferCommand>
    {
        public AddOfferValidator()
        {
            RuleFor(sp => sp.OfferDtoRequest.SalePostId).NotEmpty();
            RuleFor(sp => sp.OfferDtoRequest.BuyerId).NotEmpty();
        }
    }

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand>
    {
        private readonly ISalePostRepository _repositorySalePost;
        private readonly IOfferRepository _repositoryOffer;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<UserById> _requestClient;
        private readonly ILogger<CreateOfferCommandHandler> _logger;

        public CreateOfferCommandHandler(IOfferRepository repositoryOffer, ISalePostRepository repositorySalePost, IMapper mapper, IPublishEndpoint publishEndpoint, IRequestClient<UserById> requestClient, ILogger<CreateOfferCommandHandler> logger)
        {
            _repositorySalePost = repositorySalePost;
            _repositoryOffer = repositoryOffer;
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
            _requestClient = requestClient;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var salePost = await _repositorySalePost.GetByGUIDAsync(new Guid(request.OfferDtoRequest.SalePostId), cancellationToken);
                var offerExisting = await _repositoryOffer.GetOfferExistingByMerchPostId(new Guid(request.OfferDtoRequest.SalePostId), request.OfferDtoRequest.BuyerId, cancellationToken);
                if (offerExisting != null)
                {
                    offerExisting.OfferStatePostId = 'A';
                }
                var offer = new OfferPost
                {
                    MerchPostId = new Guid(request.OfferDtoRequest.SalePostId),
                    BuyerId = request.OfferDtoRequest.BuyerId,
                    OfferStatePostId = 'C',
                    Price = request.OfferDtoRequest.Price,
                    SellerId = salePost.UserId
                };

                await _repositoryOffer.ExecuteInTransactionAsync(async () =>
                {
                    if (offerExisting != null)
                    {
                        await _repositoryOffer.UpdateAsync(offerExisting, cancellationToken);
                    }

                    // Ajout de l'offre dans la base de données
                    var idOffer = await _repositoryOffer.CreateOffer(offer, cancellationToken);


                    var userVendeurById = new UserById(salePost.UserId);
                    var userVendeurFromAuth = await _requestClient.GetResponse<UserByIdResponse>(userVendeurById, cancellationToken);

                    var userAcheteurById = new UserById(request.OfferDtoRequest.BuyerId);
                    var userAcheteurFromAuth = await _requestClient.GetResponse<UserByIdResponse>(userAcheteurById, cancellationToken);
                    
                    //Rabbit
                    var users = new List<user>
                    {
                        // Vendeur
                        new user
                        {
                            id = userVendeurFromAuth.Message.idUser,
                            userName = userVendeurFromAuth.Message.username
                        },

                        // Acheteur
                        new user
                        {
                            id = userAcheteurFromAuth.Message.idUser,
                            userName = userAcheteurFromAuth.Message.username
                        }
                    };  

                    var message = new message
                    {
                        idUserEnvoi = request.OfferDtoRequest.BuyerId,
                        dateEnvoi = DateTime.Now,
                        texte = "",
                        offre = new offre
                        {
                            id = idOffer,
                            etat = "C",
                            prixPropose = request.OfferDtoRequest.Price
                        }
                    };
                    var offerMessage = new AddMessage(JsonSerializer.Serialize(users), JsonSerializer.Serialize(message), request.OfferDtoRequest.SalePostId);
                    await _publishEndpoint.Publish(offerMessage, cancellationToken);

                }, cancellationToken);
                offerExisting = await _repositoryOffer.GetOfferExistingByMerchPostId(new Guid(request.OfferDtoRequest.SalePostId), request.OfferDtoRequest.BuyerId, cancellationToken);
                if (offerExisting != null)
                {
                    _logger.LogInformation("Offer created with id {Id}", offerExisting.Id);
                }
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError("Error during creation of offer with error {error}", e.Message);
                throw;
            }
        }
    }
}
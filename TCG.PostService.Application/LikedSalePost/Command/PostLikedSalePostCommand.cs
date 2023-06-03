using FluentValidation;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSalePost.DTO.Request;
using TCG.PostService.Application.LikedSalePost.DTO.Response;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.LikedSalePost.Command
{
    public record PostLikedSalePostCommand(LikedSalePostDtoRequest LikedSalePostDtoRequest) : IRequest<LikedSalePostDtoResponse>;

    public class PostLikedSalePostValidator : AbstractValidator<PostLikedSalePostCommand>
    {
        public PostLikedSalePostValidator()
        {
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.UserId).NotEmpty();
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.SalePostId).NotEmpty();
        }
    }

    public class PostLikedSalePostCommandHandler : IRequestHandler<PostLikedSalePostCommand, LikedSalePostDtoResponse>
    {
        private readonly ILogger<PostLikedSalePostCommandHandler> _logger;
        private readonly ILikedSalePostRepository _likedSalePostRepository;
        private readonly ISalePostRepository _salePostRepository;
        private readonly IRequestClient<PostCreated> _requestClient;
        private readonly IMapper _mapper;

        public PostLikedSalePostCommandHandler(ISalePostRepository salePostRepository, IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<PostLikedSalePostCommandHandler> logger, ILikedSalePostRepository likedSalePostRepository)
        {
            _requestClient = requestClient;
            _logger = logger;
            _likedSalePostRepository = likedSalePostRepository;
            _salePostRepository = salePostRepository;
            _mapper = mapper;
        }
        

        public async Task<LikedSalePostDtoResponse> Handle(PostLikedSalePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LikedSalePosts likedSalePost = new()
                {
                    LikeAt = DateTime.Now,
                    SalePostId = request.LikedSalePostDtoRequest.SalePostId,
                    UserId = request.LikedSalePostDtoRequest.UserId
                };

                // Récupérez le SalePost correspondant à partir de la base de données.
                var salePost = await _salePostRepository.GetSingleSalePostAsync(cancellationToken, likedSalePost.SalePostId);

                // Assurez-vous que SalePost a été trouvé, sinon lancez une exception ou gérez l'erreur comme vous le souhaitez.
                if (salePost == null)
                {
                    throw new Exception($"No SalePost found with ID {likedSalePost.SalePostId}");
                }

                // Affectez SalePost à likedSalePost.
                likedSalePost.SalePost = salePost;
                await _likedSalePostRepository.AddAsync(likedSalePost, cancellationToken);
                _logger.LogInformation("Creating a like for user {uid} for Sale post {Id}", likedSalePost.UserId, likedSalePost.SalePostId);

                var mapped = _mapper.Map<LikedSalePostDtoResponse>(likedSalePost);
                return mapped;
            }
            catch(Exception e)
            {
                _logger.LogError("Error during creation of like for Sale post with error {error}", e.Message);
                throw;
            }
        }
    }
}

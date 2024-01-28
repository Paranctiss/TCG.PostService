using System.Collections;
using System.Linq.Expressions;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;

namespace TCG.PostService.Application.SalePost.Query;

public record GetSalePostPublicQuery(string idReference, string[] idExtensions, string[] idGradings, string idUser, int pageNumber, int pageSize, string token) : IRequest<IEnumerable<SalePostDtoResponse>>;

public class GetSalePostPublicQueryHandler : IRequestHandler<GetSalePostPublicQuery, IEnumerable<SalePostDtoResponse>>
{
    private readonly ILogger<GetSalePostPublicQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IPictureRepository _pictureRepository;
    private readonly ILikedSalePostRepository _likedSalePostRepository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserByToken> _requestClient;

    public GetSalePostPublicQueryHandler(
        ILogger<GetSalePostPublicQueryHandler> logger, 
        ISalePostRepository repository, 
        IMapper mapper,
        IPictureRepository pictureRepository,
        ILikedSalePostRepository likedSalePostRepository,
        IRequestClient<UserByToken> requestClient)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _pictureRepository = pictureRepository;
        _likedSalePostRepository = likedSalePostRepository;
        _requestClient = requestClient;
    }
    public async Task<IEnumerable<SalePostDtoResponse>> Handle(GetSalePostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool isOwner = false;
            int idUserFromAuth = 0;
            Expression<Func<Domain.SalePost, bool>> filter;
            if (request.token != "")
            {
                var userByToken = new UserByToken(request.token, cancellationToken);
                try
                {
                   var userFromAuth = await _requestClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);
                    if (userFromAuth != null) {
                        idUserFromAuth = userFromAuth.Message.idUser;
                        if(idUserFromAuth == int.Parse(request.idUser))
                        {
                            isOwner = true;
                        }
                        else
                        {
                            isOwner = false;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
            }

            if(isOwner)
            {
                filter = x => x.StatePostId == 'C';
            }
            else
            {
                filter = x => x.IsPublic && x.StatePostId == 'C';
            }
        var salePost = await _repository.GetAllSalePostPublicAsync(
                request.idReference,
                request.idExtensions,
                request.idGradings,
                request.idUser,
                request.pageNumber,
                request.pageSize,
                cancellationToken,
                orderBy: x => x.CreatedAt,
                descending: true,
                filter: filter);

            if (salePost == null)
            {
                _logger.LogWarning("No public sale post found");
                return null;
            }

            var salePostDto = _mapper.Map<List<SalePostDtoResponse>>(salePost);

            if(idUserFromAuth != 0)
            {
                foreach (SalePostDtoResponse salePostDtoResponse in salePostDto)
                {
                    if (_likedSalePostRepository.IsSalePostLiked(cancellationToken, idUserFromAuth, salePostDtoResponse.Id))
                    {
                        salePostDtoResponse.Liked = true;
                    }
                    else
                    {
                        salePostDtoResponse.Liked = false;
                    }
                }
            }
            

            return salePostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
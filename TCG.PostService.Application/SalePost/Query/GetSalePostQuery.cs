using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Response;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Application.SearchPost.Query;

namespace TCG.PostService.Application.SalePost.Query;


public record GetSalePostQuery(Guid id, string token) : IRequest<SalePostDtoResponse>;

public class GetSalePostQueryHandler : IRequestHandler<GetSalePostQuery, SalePostDtoResponse>
{
    private readonly ILogger<GetSalePostQueryHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserById> _requestUserIdClient;
    private readonly IRequestClient<UserByToken> _requestUserTokenClient;
    private readonly ILikedSalePostRepository _likedSalePostRepository;

    public GetSalePostQueryHandler(ILogger<GetSalePostQueryHandler> logger, ISalePostRepository repository, IMapper mapper, IRequestClient<UserById> requestUserIdClient, IRequestClient<UserByToken> requestUserTokenClient, ILikedSalePostRepository likedSalePostRepository)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _requestUserIdClient = requestUserIdClient;
        _requestUserTokenClient = requestUserTokenClient;
        _likedSalePostRepository = likedSalePostRepository;
    }
    public async Task<SalePostDtoResponse> Handle(GetSalePostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var salePost = await _repository.GetSingleSalePostAsync(cancellationToken, request.id);

            if (salePost == null)
            {
                _logger.LogWarning("Search post with id {SalePostId} not found", request.id);
                return null;
            }

            var userById = new UserById(salePost.UserId);
            var userFromAuth = await _requestUserIdClient.GetResponse<UserByIdResponse>(userById, cancellationToken);

            var salePostDtoResponse = _mapper.Map<SalePostDtoResponse>(salePost);

            salePostDtoResponse.Username = userFromAuth.Message.username;

            int idUserRequesFromAuth = 0;
            Expression<Func<Domain.SalePost, bool>> filter;
            if (request.token != "")
            {
                var userByToken = new UserByToken(request.token, cancellationToken);
                try
                {
                    var userRequestFromAuth = await _requestUserTokenClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);
                    if (userRequestFromAuth != null)
                    {
                        idUserRequesFromAuth = userRequestFromAuth.Message.idUser;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
            }

            if (idUserRequesFromAuth != 0)
            {
                if(salePost.UserId == idUserRequesFromAuth)
                {
                    salePostDtoResponse.IsOwner = true;
                }
                else
                {
                    salePostDtoResponse.IsOwner = false;
                }

                    if (_likedSalePostRepository.IsSalePostLiked(cancellationToken, idUserRequesFromAuth, salePostDtoResponse.Id))
                    {
                        salePostDtoResponse.Liked = true;
                    }
                    else
                    {
                        salePostDtoResponse.Liked = false;
                    }
            }

            return salePostDtoResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System.Linq.Expressions;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostPublicQuery(string idReference, string[] idExtensions, string[] idGradings, string idUser, int pageNumber, int pageSize, string token) : IRequest<IEnumerable<SearchPostDtoResponse>>;

public class GetSearchPostPublicQueryHandler : IRequestHandler<GetSearchPostPublicQuery, IEnumerable<SearchPostDtoResponse>>
{
    private readonly ILogger<GetSearchPostPublicQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly ILikedSearchPostRepository _likedSearchPostRepository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserByToken> _requestClient;


    public GetSearchPostPublicQueryHandler(
        ILogger<GetSearchPostPublicQueryHandler> logger, 
        ISearchPostRepository repository, 
        ILikedSearchPostRepository likedSearchPostRepository, 
        IMapper mapper, 
        IRequestClient<UserByToken> requestClient)
    {
        _logger = logger;
        _repository = repository;
        _likedSearchPostRepository = likedSearchPostRepository;
        _mapper = mapper;
        _requestClient = requestClient;
    }
    public async Task<IEnumerable<SearchPostDtoResponse>> Handle(GetSearchPostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            bool isOwner = false;
            int idUserFromAuth = 0;

            Expression<Func<Domain.SearchPost, bool>> filter;
            if (request.token != "")
            {
                var userByToken = new UserByToken(request.token, cancellationToken);
                try
                {
                    var userFromAuth = await _requestClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);
                    if (userFromAuth != null)
                    {
                        idUserFromAuth = userFromAuth.Message.idUser;
                        if (idUserFromAuth == int.Parse(request.idUser))
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
            if (isOwner)
            {
                filter = x => x.StatePostId == 'C';
            }
            else
            {
                filter = x => x.IsPublic && x.StatePostId == 'C';
            }

            var searchPosts = await _repository.GetAllSearchPostPublicAsync(
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

            if (searchPosts == null)
            {
                _logger.LogWarning("No public search post found");
                return null;
            }

            var searchPostDto = _mapper.Map<List<SearchPostDtoResponse>>(searchPosts);
            if (request.token != "")
            {
                try
                {
                    var userByToken = new UserByToken(request.token, cancellationToken);
                    var userFromAuth = await _requestClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);

                    foreach (SearchPostDtoResponse searchPostDtoResponse in searchPostDto)
                    {
                        if (_likedSearchPostRepository.IsSearchPostLiked(cancellationToken, userFromAuth.Message.idUser, searchPostDtoResponse.Id))
                        {
                            searchPostDtoResponse.Liked = true;
                        }
                        else
                        {
                            searchPostDtoResponse.Liked = false;
                        }
                    }
                }catch(Exception e)
                {

                }
                finally
                {

                }
                
            }

            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
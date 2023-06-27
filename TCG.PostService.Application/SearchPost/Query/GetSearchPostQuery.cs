using MapsterMapper;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostQuery(Guid id, string token) : IRequest<SearchPostDtoResponse>;

public class GetSearchPostQueryHandler : IRequestHandler<GetSearchPostQuery, SearchPostDtoResponse>
{
    private readonly ILogger<GetSearchPostQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserById> _requestClient;
    private readonly ILikedSearchPostRepository _likedRepository;
    private readonly IRequestClient<UserByToken> _requestUserTokenClient;

    public GetSearchPostQueryHandler(ILogger<GetSearchPostQueryHandler> logger, ISearchPostRepository repository, IMapper mapper, IRequestClient<UserById> requestClient, IRequestClient<UserByToken> requestUserTokenClient, ILikedSearchPostRepository likedRepository)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _requestClient = requestClient;
        _requestUserTokenClient = requestUserTokenClient;
        _likedRepository = likedRepository;
    }
    
    public async Task<SearchPostDtoResponse> Handle(GetSearchPostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPost = await _repository.GetSingleSearchPostAsync(cancellationToken, request.id);

            if (searchPost == null)
            {
                _logger.LogWarning("Search post with id {SearchPostId} not found", request.id);
                return null;
            }

            var userById = new UserById(searchPost.UserId);
            var userFromAuth = await _requestClient.GetResponse<UserByIdResponse>(userById, cancellationToken);

            var searchPostDto = _mapper.Map<SearchPostDtoResponse>(searchPost);

            searchPostDto.Username = userFromAuth.Message.username;


            int idUserRequesFromAuth = 0;
            Expression<Func<Domain.SearchPost, bool>> filter;
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

                if (_likedRepository.IsSearchPostLiked(cancellationToken, idUserRequesFromAuth, searchPostDto.Id))
                {
                    searchPostDto.Liked = true;
                }
                else
                {
                    searchPostDto.Liked = false;
                }
            }



            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}
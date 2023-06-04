using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostPublicQuery(string idReference) : IRequest<IEnumerable<SearchPostDtoResponse>>;

public class GetSearchPostPublicQueryHandler : IRequestHandler<GetSearchPostPublicQuery, IEnumerable<SearchPostDtoResponse>>
{
    private readonly ILogger<GetSearchPostPublicQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly ILikedSearchPostRepository _likedSearchPostRepository;
    private readonly IMapper _mapper;

    public GetSearchPostPublicQueryHandler(ILogger<GetSearchPostPublicQueryHandler> logger, ISearchPostRepository repository, ILikedSearchPostRepository likedSearchPostRepository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _likedSearchPostRepository = likedSearchPostRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SearchPostDtoResponse>> Handle(GetSearchPostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPosts = await _repository.GetAllSearchPostPublicAsync(request.idReference, cancellationToken);

            if (searchPosts == null)
            {
                _logger.LogWarning("No public search post found");
                return null;
            }

            var searchPostDto = _mapper.Map<List<SearchPostDtoResponse>>(searchPosts);

            foreach(SearchPostDtoResponse searchPostDtoResponse in searchPostDto)
            {
                if(_likedSearchPostRepository.IsSearchPostLiked(cancellationToken, 1, searchPostDtoResponse.Id))
                {
                    searchPostDtoResponse.Liked = true;
                }
                else
                {
                    searchPostDtoResponse.Liked = false;
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
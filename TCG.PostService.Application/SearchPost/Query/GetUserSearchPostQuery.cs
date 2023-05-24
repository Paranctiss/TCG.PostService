using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetUserSearchPostQuery(int id, int nbMax) : IRequest<IEnumerable<SearchPostDtoResponse>>;

public class GetUserSearchPostQueryHandler : IRequestHandler<GetUserSearchPostQuery, IEnumerable<SearchPostDtoResponse>>
{
    private readonly ILogger<GetUserSearchPostQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly ILikedSearchPostRepository _likedSearchPostRepository;
    private readonly IMapper _mapper;

    public GetUserSearchPostQueryHandler(ILogger<GetUserSearchPostQueryHandler> logger, ISearchPostRepository repository, ILikedSearchPostRepository likedSearchPostRepository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _likedSearchPostRepository = likedSearchPostRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SearchPostDtoResponse>> Handle(GetUserSearchPostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPosts = await _repository.GetLastUserSearchPostAsync(request.id, request.nbMax, cancellationToken);

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
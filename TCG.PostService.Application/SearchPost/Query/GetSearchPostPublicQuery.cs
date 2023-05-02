using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostPublicQuery() : IRequest<IEnumerable<SearchPostDtoResponse>>;

public class GetSearchPostPublicQueryHandler : IRequestHandler<GetSearchPostPublicQuery, IEnumerable<SearchPostDtoResponse>>
{
    private readonly ILogger<GetSearchPostPublicQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IGradingRepository _gradingRepository;
    private readonly IMapper _mapper;

    public GetSearchPostPublicQueryHandler(ILogger<GetSearchPostPublicQueryHandler> logger, ISearchPostRepository repository, IGradingRepository gradingRepository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _gradingRepository = gradingRepository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<SearchPostDtoResponse>> Handle(GetSearchPostPublicQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPosts = await _repository.GetAllSearchPostPublicAsync(cancellationToken);

            if (searchPosts == null)
            {
                _logger.LogWarning("No public search post found");
                return null;
            }

            var searchPostDto = _mapper.Map<IEnumerable<SearchPostDtoResponse>>(searchPosts);

            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with public : {ErrorMessage}", ex.Message);
            throw;
        }
    }
}
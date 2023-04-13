using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostQuery(int id) : IRequest<SearchPostDtoResponse>;

public class GetSearchPostQueryHandler : IRequestHandler<GetSearchPostQuery, SearchPostDtoResponse>
{
    private readonly ILogger<GetSearchPostQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;

    public GetSearchPostQueryHandler(ILogger<GetSearchPostQueryHandler> logger, ISearchPostRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<SearchPostDtoResponse> Handle(GetSearchPostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPost = await _repository.GetByIdAsync(request.id, cancellationToken);

            if (searchPost == null)
            {
                _logger.LogWarning("Search post with id {SearchPostId} not found", request.id);
                return null;
            }

            var searchPostDto = _mapper.Map<SearchPostDtoResponse>(searchPost);

            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostQuery(int id) : IRequest<SearchPostDto>;

public class GetSearchPostQueryHandler : IRequestHandler<GetSearchPostQuery, SearchPostDto>
{
    private readonly ILogger<CreateSearchPostCommandHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;

    public GetSearchPostQueryHandler(ILogger<CreateSearchPostCommandHandler> logger, ISearchPostRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<SearchPostDto> Handle(GetSearchPostQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var searchPost = await _repository.GetByIdAsync(request.id, cancellationToken);

            if (searchPost == null)
            {
                _logger.LogWarning("Search post with id {SearchPostId} not found", request.id);
                return null;
            }

            var searchPostDto = _mapper.Map<SearchPostDto>(searchPost);

            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}
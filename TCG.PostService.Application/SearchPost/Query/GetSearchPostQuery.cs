using MapsterMapper;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.SearchPost.Query;

public record GetSearchPostQuery(Guid id) : IRequest<SearchPostDtoResponse>;

public class GetSearchPostQueryHandler : IRequestHandler<GetSearchPostQuery, SearchPostDtoResponse>
{
    private readonly ILogger<GetSearchPostQueryHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserById> _requestClient;

    public GetSearchPostQueryHandler(ILogger<GetSearchPostQueryHandler> logger, ISearchPostRepository repository, IMapper mapper, IRequestClient<UserById> requestClient)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _requestClient = requestClient;
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

            return searchPostDto;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving search post with id {SearchPostId}: {ErrorMessage}", request.id, ex.Message);
            throw;
        }
    }
}
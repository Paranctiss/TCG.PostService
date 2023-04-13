using FluentValidation;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Command;

public record CreateSearchPostCommand(SearchPostDtoRequest SearchPostDtoRequest) : IRequest<SearchPostDtoResponse>;

public class AddSearchPostValidator : AbstractValidator<CreateSearchPostCommand>
{
    public AddSearchPostValidator()
    {
        _ = RuleFor(sp => sp.SearchPostDtoRequest.ItemId).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDtoRequest.Price).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDtoRequest.IsPublic).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDtoRequest.StatePostId).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDtoRequest.UserId).NotEmpty();
    }
}
public class CreateSearchPostCommandHandler : IRequestHandler<CreateSearchPostCommand, SearchPostDtoResponse>
{
    private readonly ILogger<CreateSearchPostCommandHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IRequestClient<PostCreated> _requestClient;
    private readonly IMapper _mapper;

    public CreateSearchPostCommandHandler(IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<CreateSearchPostCommandHandler> logger, ISearchPostRepository dbService)
    {
        _requestClient = requestClient;
        _logger = logger;
        _repository = dbService;
        _mapper = mapper;
    }
    public async Task<SearchPostDtoResponse> Handle(CreateSearchPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var postCreatedMessage = new PostCreated(request.SearchPostDtoRequest.ItemId);
            var itemFromCatalog = await _requestClient.GetResponse<PostCreatedResponse>(postCreatedMessage, cancellationToken);

            Domain.SearchPost searchPost = new()
            {
                ItemId = request.SearchPostDtoRequest.ItemId,
                Price = request.SearchPostDtoRequest.Price,
                Remarks = request.SearchPostDtoRequest.Remarks,
                IsPublic = request.SearchPostDtoRequest.IsPublic,
                StatePostId = request.SearchPostDtoRequest.StatePostId,
                UserId = request.SearchPostDtoRequest.UserId,
                Image = itemFromCatalog.Message.Image,
                Name = itemFromCatalog.Message.Name
            };

            await _repository.AddAsync(searchPost, cancellationToken);
            _logger.LogInformation("Creating a search post with serch post id {Id}", searchPost.Id);
            return _mapper.Map<SearchPostDtoResponse>(searchPost); 
        }
        catch (Exception e)
        {
            _logger.LogError("Error during creation of search post with error {error}", e.Message);
            throw;
        }
    }
}
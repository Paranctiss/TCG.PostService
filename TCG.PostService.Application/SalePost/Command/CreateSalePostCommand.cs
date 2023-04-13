using FluentValidation;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.Common.Contracts;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SalePost.DTO.Request;
using TCG.PostService.Application.SalePost.DTO.Response;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.SalePost.Command;

public record CreateSalePostCommand(SalePostDtoRequest SalePostDtoRequest) : IRequest<SalePostDtoResponse>;
public class AddSalePostValidator : AbstractValidator<CreateSalePostCommand>
{
    public AddSalePostValidator()
    {
        _ = RuleFor(sp => sp.SalePostDtoRequest.UserId).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.Price).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.StatePostId).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.Remarks).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.GradingId).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.IsPublic).NotEmpty();
        _ = RuleFor(sp => sp.SalePostDtoRequest.ItemId).NotEmpty();
    }
}
public class CreateSalePostCommandHandler : IRequestHandler<CreateSalePostCommand, SalePostDtoResponse>
{
    private readonly IRequestClient<PostCreated> _requestClient;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateSalePostCommandHandler> _logger;

    public CreateSalePostCommandHandler(IRequestClient<PostCreated> requestClient, ISalePostRepository repository, IMapper mapper, ILogger<CreateSalePostCommandHandler> logger)
    {
        _requestClient = requestClient;
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<SalePostDtoResponse> Handle(CreateSalePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var salePostCreatedMessage = new PostCreated(request.SalePostDtoRequest.ItemId);
            var itemFromCatalog = await _requestClient.GetResponse<PostCreatedResponse>(salePostCreatedMessage, cancellationToken);

            Domain.SalePost salePost = new()
            {
                GradingId = request.SalePostDtoRequest.GradingId,
                ItemId = request.SalePostDtoRequest.ItemId,
                Image = itemFromCatalog.Message.Image,
                IsPublic = request.SalePostDtoRequest.IsPublic,
                Remarks = request.SalePostDtoRequest.Remarks,
                Name = itemFromCatalog.Message.Name,
                Price = request.SalePostDtoRequest.Price,
                StatePostId = request.SalePostDtoRequest.StatePostId,
                UserId = request.SalePostDtoRequest.UserId
            };

            await _repository.AddAsync(salePost, cancellationToken);
            _logger.LogInformation("Creating a search post with sale post id {Id}", salePost.Id);
            return _mapper.Map<SalePostDtoResponse>(salePost);
        }
        catch (Exception e)
        {
            _logger.LogError("Error during creation of search post with error {error}", e.Message);
            throw;
        }
    }
}
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.Offer.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.SearchPost.Command;

public record CreateSearchPostCommand(SearchPostDto SearchPostDto) : IRequest<SearchPostDto>;

public class AddSearchPostValidator : AbstractValidator<CreateSearchPostCommand>
{
    public AddSearchPostValidator()
    {
        _ = RuleFor(sp => sp.SearchPostDto.Id).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDto.ItemId).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDto.Price).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDto.IsPublic).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDto.StatePostId).NotEmpty();
        _ = RuleFor(sp => sp.SearchPostDto.UserId).NotEmpty();
    }
}
public class CreateSearchPostCommandHandler : IRequestHandler<CreateSearchPostCommand, SearchPostDto>
{
    private readonly ILogger<CreateSearchPostCommandHandler> _logger;
    private readonly ISearchPostRepository _repository;

    public CreateSearchPostCommandHandler(ILogger<CreateSearchPostCommandHandler> logger, ISearchPostRepository dbService)
    {
        _logger = logger;
        _repository = dbService;
    }
    public async Task<SearchPostDto> Handle(CreateSearchPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Domain.SearchPost searchPost = new()
            {
                Id = request.SearchPostDto.Id,
                ItemId = request.SearchPostDto.ItemId,
                Price = request.SearchPostDto.Price,
                Remarks = request.SearchPostDto.Remarks,
                IsPublic = request.SearchPostDto.IsPublic,
                StatePostId = request.SearchPostDto.StatePostId,
                UserId = request.SearchPostDto.UserId
            };

            await _repository.AddAsync(searchPost, cancellationToken);
            _logger.LogInformation("Creating a search post with sale post id {Id}", searchPost.Id);
            return request.SearchPostDto;
        }
        catch (Exception e)
        {
            _logger.LogError("Error during creation of search post with error {error}", e.Message);
            throw;
        }
    }
}
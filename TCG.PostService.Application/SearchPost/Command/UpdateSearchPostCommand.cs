using FluentValidation;
using MapsterMapper;
using MassTransit;
using MassTransit.Clients;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Command;

public record UpdateSearchPostCommand(Guid IdPost) : IRequest<SearchPostDtoResponse>;

public class UpdateSearchPostValidator : AbstractValidator<UpdateSearchPostCommand>
{
   public UpdateSearchPostValidator()
    {
        _ = RuleFor(x => x.IdPost).NotNull();
        _ = RuleFor(x => x.IsPublic).NotNull();
    }
}

public class UpdateSearchPostHandler : IRequestHandler<UpdateSearchPostCommand, SearchPostDtoResponse>
{
    private readonly ILogger<UpdateSearchPostHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;

    public UpdateSearchPostHandler(IMapper mapper, ILogger<UpdateSearchPostHandler> logger, ISearchPostRepository dbService)
    {
        _logger = logger;
        _repository = dbService;
        _mapper = mapper;
    }

    public async Task<SearchPostDtoResponse> Handle(UpdateSearchPostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Recherche du search post concerné");
            Domain.SearchPost? searchPost = await _repository.GetByGUIDAsync(request.IdPost, cancellationToken);
            
            
            _logger.LogInformation("Update du searchPost");
            searchPost.IsPublic = !searchPost.IsPublic;
            await _repository.UpdateAsync(searchPost, cancellationToken);

            var mapped = _mapper.Map<SearchPostDtoResponse>(searchPost);
            return mapped;
        }
        catch (Exception e)
        {
            _logger.LogError("Error during update of search post with error {error}", e.Message);
            throw;
        }
    }

}


   
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
using TCG.Common.Middlewares.MiddlewareException;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;
using TCG.PostService.Domain;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace TCG.PostService.Application.SearchPost.Command;

public record UpdateSearchPostCommand(Guid IdPost, string Token) : IRequest<SearchPostDtoResponse>;

public class UpdateSearchPostValidator : AbstractValidator<UpdateSearchPostCommand>
{
   public UpdateSearchPostValidator()
    {
        _ = RuleFor(x => x.IdPost).NotNull();
    }
}

public class UpdateSearchPostHandler : IRequestHandler<UpdateSearchPostCommand, SearchPostDtoResponse>
{
    private readonly ILogger<UpdateSearchPostHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserByToken> _requestClient;

    public UpdateSearchPostHandler(IMapper mapper, ILogger<UpdateSearchPostHandler> logger, ISearchPostRepository dbService, IRequestClient<UserByToken> requestClient)
    {
        _logger = logger;
        _repository = dbService;
        _mapper = mapper;
        _requestClient = requestClient;
    }

    public async Task<SearchPostDtoResponse> Handle(UpdateSearchPostCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var searchPost = await _repository.GetByGUIDAsync(request.IdPost, cancellationToken);
            searchPost.AccessCode = new Random().Next(100000, 1000000).ToString();

            if (searchPost == null)
            {
                _logger.LogWarning("Search post with id {SearchPostId} not found", request.IdPost);
                return null;
            }
            else
            {
                var userByToken = new UserByToken(request.Token, cancellationToken);
                var userFromAuth = await _requestClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);

                if (userFromAuth.Message.idUser == searchPost.UserId)
                {
                    searchPost.IsPublic = !searchPost.IsPublic;
                    await _repository.UpdateAsync(searchPost, cancellationToken);
                }
            }

            var SearchPostDtoResponse = _mapper.Map<SearchPostDtoResponse>(searchPost);

            var mapped = _mapper.Map<SearchPostDtoResponse>(searchPost);
            return mapped;

        }
        catch (Exception e)
        {
            if (e.Message.Contains("401"))
            {
                throw new UnAuthorizedException("User is unauthorized");
            }else
            {
                _logger.LogError("Error during update of search post with error {error}", e.Message);
                throw new UnAuthorizedException(e.Message);
            }
        }
    }

}
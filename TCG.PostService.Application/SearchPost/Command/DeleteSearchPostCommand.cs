using FluentValidation;
using MapsterMapper;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MassTransit.Messages;
using TCG.Common.Middlewares.MiddlewareException;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.SearchPost.DTO.Response;

namespace TCG.PostService.Application.SearchPost.Command;
public record DeleteSearchPostCommand(Guid IdPost, string Token) : IRequest<Domain.SearchPost>;

public class DeleteSearchPostValidator : AbstractValidator<DeleteSearchPostCommand>
{
    public DeleteSearchPostValidator()
    {
        _ = RuleFor(x => x.IdPost).NotNull();
    }
}

public class DeleteSearchPostHandler : IRequestHandler<DeleteSearchPostCommand, Domain.SearchPost>
{
    private readonly ILogger<DeleteSearchPostHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserByToken> _requestClient;

    public DeleteSearchPostHandler(IMapper mapper, ILogger<DeleteSearchPostHandler> logger, ISearchPostRepository dbService, IRequestClient<UserByToken> requestClient)
    {
        _logger = logger;
        _repository = dbService;
        _mapper = mapper;
        _requestClient = requestClient;
    }

    public async Task<Domain.SearchPost> Handle(DeleteSearchPostCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var searchPost = await _repository.GetByGUIDAsync(request.IdPost, cancellationToken);

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
                    await _repository.RemoveByGUIDAsync(searchPost.Id, cancellationToken);
                }
            }
            return searchPost;

        }
        catch (Exception e)
        {
            if (e.Message.Contains("401"))
            {
                throw new UnAuthorizedException("User is unauthorized");
            }
            else
            {
                _logger.LogError("Error during delete of search post with error {error}", e.Message);
                throw new UnAuthorizedException(e.Message);
            }
        }
    }

}
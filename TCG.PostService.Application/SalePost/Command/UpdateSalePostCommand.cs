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
using TCG.PostService.Application.SalePost.DTO.Response;

namespace TCG.PostService.Application.SalePost.Command;
public record UpdateSalePostCommand(Guid IdPost, string Token) : IRequest<Domain.SalePost>;

public class UpdateSalePostValidator : AbstractValidator<UpdateSalePostCommand>
{
    public UpdateSalePostValidator()
    {
        _ = RuleFor(x => x.IdPost).NotNull();
    }
}

public class UpdateSalePostHandler : IRequestHandler<UpdateSalePostCommand, Domain.SalePost>
{
    private readonly ILogger<UpdateSalePostHandler> _logger;
    private readonly ISalePostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserByToken> _requestClient;

    public UpdateSalePostHandler(IMapper mapper, ILogger<UpdateSalePostHandler> logger, ISalePostRepository dbService, IRequestClient<UserByToken> requestClient)
    {
        _logger = logger;
        _repository = dbService;
        _mapper = mapper;
        _requestClient = requestClient;
    }

    public async Task<Domain.SalePost> Handle(UpdateSalePostCommand request, CancellationToken cancellationToken)
    {
        try
        {

            var salePost = await _repository.GetByGUIDAsync(request.IdPost, cancellationToken);
            salePost.AccessCode = new Random().Next(100000, 1000000).ToString();
            
            if (salePost == null)
            {
                _logger.LogWarning("Sale post with id {SalePostId} not found", request.IdPost);
                return null;
            }
            else
            {
                var userByToken = new UserByToken(request.Token, cancellationToken);
                var userFromAuth = await _requestClient.GetResponse<UserByTokenResponse>(userByToken, cancellationToken);

                if (userFromAuth.Message.idUser == salePost.UserId)
                {
                    salePost.IsPublic = !salePost.IsPublic;
                    await _repository.UpdateAsync(salePost, cancellationToken);
                }
            }
            return  salePost;

        }
        catch (Exception e)
        {
            if (e.Message.Contains("401"))
            {
                throw new UnAuthorizedException("User is unauthorized");
            }
            else
            {
                _logger.LogError("Error during update of sale post with error {error}", e.Message);
                throw new UnAuthorizedException(e.Message);
            }
        }
    }

}
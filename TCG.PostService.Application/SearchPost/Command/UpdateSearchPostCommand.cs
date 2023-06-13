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
using TCG.PostService.Application.SalePost.Query;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.SearchPost.Command;

public record UpdateSearchPostCommand(Guid IdPost, int IdUser) : IRequest<SearchPostDtoResponse>;

public class UpdateSearchPostValidator : AbstractValidator<UpdateSearchPostCommand>
{
   public UpdateSearchPostValidator()
    {
        _ = RuleFor(x => x.IdPost).NotNull();
        _ = RuleFor(x => x.IdUser).NotNull();
    }
}

public class UpdateSearchPostHandler : IRequestHandler<UpdateSearchPostCommand, SearchPostDtoResponse>
{
    private readonly ILogger<UpdateSearchPostHandler> _logger;
    private readonly ISearchPostRepository _repository;
    private readonly IMapper _mapper;
    private readonly IRequestClient<UserById> _requestClient;

    public UpdateSearchPostHandler(IMapper mapper, ILogger<UpdateSearchPostHandler> logger, ISearchPostRepository dbService, IRequestClient<UserById> requestClient)
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

            if (searchPost == null)
            {
                _logger.LogWarning("Search post with id {SearchPostId} not found", request.IdPost);
                return null;
            }

            //var userById = new UserById(searchPost.UserId);
            //var userFromAuth = await _requestClient.GetResponse<UserByIdResponse>(userById, cancellationToken);

            searchPost.IsPublic = !searchPost.IsPublic;
            await _repository.UpdateAsync(searchPost, cancellationToken);

            var SearchPostDtoResponse = _mapper.Map<SearchPostDtoResponse>(searchPost);

            //SearchPostDtoResponse.UserId = userFromAuth.Message.idUser;

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
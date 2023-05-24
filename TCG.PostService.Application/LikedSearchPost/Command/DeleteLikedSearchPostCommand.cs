using FluentValidation;
using MapsterMapper;
using MassTransit;
using MassTransit.JobService.Components;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.Common.MassTransit.Messages;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSearchPost.DTO.Request;
using TCG.PostService.Application.LikedSearchPost.DTO.Response;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.DTO;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.LikedSearchPost.Command
{
    public record DeleteLikedSearchPostCommand(LikedSearchPostDtoRequest LikedSearchPostDtoRequest) : IRequest<LikedSearchPostDtoResponse>;

    public class DeleteLikedSearchPostValidator : AbstractValidator<DeleteLikedSearchPostCommand>
    {
        public DeleteLikedSearchPostValidator()
        {
            _ = RuleFor(sp => sp.LikedSearchPostDtoRequest.UserId).NotEmpty();
            _ = RuleFor(sp => sp.LikedSearchPostDtoRequest.SearchPostId).NotEmpty();
        }
    }

    public class DeleteLikedSearchPostCommandHandler : IRequestHandler<DeleteLikedSearchPostCommand, LikedSearchPostDtoResponse>
    {
        private readonly ILogger<DeleteLikedSearchPostCommand> _logger;
        private readonly ILikedSearchPostRepository _likedSearchPostRepository;
        private readonly IRequestClient<PostCreated> _requestClient;
        private readonly IMapper _mapper;

        public DeleteLikedSearchPostCommandHandler(IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<DeleteLikedSearchPostCommand> logger, ILikedSearchPostRepository likedSearchPostRepository)
        {
            _requestClient = requestClient;
            _logger = logger;
            _likedSearchPostRepository = likedSearchPostRepository;
            _mapper = mapper;
        }
        

        public async Task<LikedSearchPostDtoResponse> Handle(DeleteLikedSearchPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _likedSearchPostRepository.RemoveLikedSearchPosts(request.LikedSearchPostDtoRequest, cancellationToken);
                _logger.LogInformation("Deleting a like for user {uid} for search post {Id}", request.LikedSearchPostDtoRequest.UserId, request.LikedSearchPostDtoRequest.SearchPostId);
                //likedSearchPost.SearchPost = new Domain.SearchPost();
                var mapped = _mapper.Map<LikedSearchPostDtoResponse>(request.LikedSearchPostDtoRequest);
                return mapped;
            }
            catch(Exception e)
            {
                _logger.LogError("Error during creation of like for search post with error {error}", e.Message);
                throw;
            }
           
        }
    }
}

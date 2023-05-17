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
    public record PostLikedSearchPostCommand(LikedSearchPostDtoRequest LikedSearchPostDtoRequest) : IRequest<LikedSearchPostDtoResponse>;

    public class PostLikedSearchPostValidator : AbstractValidator<PostLikedSearchPostCommand>
    {
        public PostLikedSearchPostValidator()
        {
            _ = RuleFor(sp => sp.LikedSearchPostDtoRequest.UserId).NotEmpty();
            _ = RuleFor(sp => sp.LikedSearchPostDtoRequest.SearchPostId).NotEmpty();
        }
    }

    public class PostLikedSearchPostCommandHandler : IRequestHandler<PostLikedSearchPostCommand, LikedSearchPostDtoResponse>
    {
        private readonly ILogger<PostLikedSearchPostCommandHandler> _logger;
        private readonly ILikedSearchPostRepository _likedSearchPostRepository;
        private readonly IRequestClient<PostCreated> _requestClient;
        private readonly IMapper _mapper;

        public PostLikedSearchPostCommandHandler(IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<PostLikedSearchPostCommandHandler> logger, ILikedSearchPostRepository likedSearchPostRepository)
        {
            _requestClient = requestClient;
            _logger = logger;
            _likedSearchPostRepository = likedSearchPostRepository;
            _mapper = mapper;
        }
        

        public async Task<LikedSearchPostDtoResponse> Handle(PostLikedSearchPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.LikedSearchPosts likedSearchPost = new()
                {
                    LikeAt = DateTime.Now,
                    SearchPostId = request.LikedSearchPostDtoRequest.SearchPostId,
                    UserId = request.LikedSearchPostDtoRequest.UserId
                };

                await _likedSearchPostRepository.AddAsync(likedSearchPost, cancellationToken);
                _logger.LogInformation("Creating a like for user {uid} for search post {Id}", likedSearchPost.UserId, likedSearchPost.SearchPostId);
                likedSearchPost.SearchPost = new Domain.SearchPost();
                var mapped = _mapper.Map<LikedSearchPostDtoResponse>(likedSearchPost);
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

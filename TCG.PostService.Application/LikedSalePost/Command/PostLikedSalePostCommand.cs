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
using TCG.PostService.Application.LikedSalePost.DTO.Request;
using TCG.PostService.Application.LikedSalePost.DTO.Response;
using TCG.PostService.Application.SalePost.Command;
using TCG.PostService.Application.SalePost.DTO;
using TCG.PostService.Domain;

namespace TCG.PostService.Application.LikedSalePost.Command
{
    public record PostLikedSalePostCommand(LikedSalePostDtoRequest LikedSalePostDtoRequest) : IRequest<LikedSalePostDtoResponse>;

    public class PostLikedSalePostValidator : AbstractValidator<PostLikedSalePostCommand>
    {
        public PostLikedSalePostValidator()
        {
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.UserId).NotEmpty();
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.SalePostId).NotEmpty();
        }
    }

    public class PostLikedSalePostCommandHandler : IRequestHandler<PostLikedSalePostCommand, LikedSalePostDtoResponse>
    {
        private readonly ILogger<PostLikedSalePostCommandHandler> _logger;
        private readonly ILikedSalePostRepository _likedSalePostRepository;
        private readonly IRequestClient<PostCreated> _requestClient;
        private readonly IMapper _mapper;

        public PostLikedSalePostCommandHandler(IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<PostLikedSalePostCommandHandler> logger, ILikedSalePostRepository likedSalePostRepository)
        {
            _requestClient = requestClient;
            _logger = logger;
            _likedSalePostRepository = likedSalePostRepository;
            _mapper = mapper;
        }
        

        public async Task<LikedSalePostDtoResponse> Handle(PostLikedSalePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Domain.LikedSalePosts likedSalePost = new()
                {
                    LikeAt = DateTime.Now,
                    SalePostId = request.LikedSalePostDtoRequest.SalePostId,
                    UserId = request.LikedSalePostDtoRequest.UserId
                };

                await _likedSalePostRepository.AddAsync(likedSalePost, cancellationToken);
                _logger.LogInformation("Creating a like for user {uid} for Sale post {Id}", likedSalePost.UserId, likedSalePost.SalePostId);
                likedSalePost.SalePost = new Domain.SalePost();
                var mapped = _mapper.Map<LikedSalePostDtoResponse>(likedSalePost);
                return mapped;
            }
            catch(Exception e)
            {
                _logger.LogError("Error during creation of like for Sale post with error {error}", e.Message);
                throw;
            }
           
        }
    }
}

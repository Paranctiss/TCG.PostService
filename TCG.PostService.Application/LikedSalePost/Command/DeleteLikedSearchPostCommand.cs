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
    public record DeleteLikedSalePostCommand(LikedSalePostDtoRequest LikedSalePostDtoRequest) : IRequest<LikedSalePostDtoResponse>;

    public class DeleteLikedSalePostValidator : AbstractValidator<DeleteLikedSalePostCommand>
    {
        public DeleteLikedSalePostValidator()
        {
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.UserId).NotEmpty();
            _ = RuleFor(sp => sp.LikedSalePostDtoRequest.SalePostId).NotEmpty();
        }
    }

    public class DeleteLikedSalePostCommandHandler : IRequestHandler<DeleteLikedSalePostCommand, LikedSalePostDtoResponse>
    {
        private readonly ILogger<DeleteLikedSalePostCommand> _logger;
        private readonly ILikedSalePostRepository _likedSalePostRepository;
        private readonly IRequestClient<PostCreated> _requestClient;
        private readonly IMapper _mapper;

        public DeleteLikedSalePostCommandHandler(IMapper mapper, IRequestClient<PostCreated> requestClient, ILogger<DeleteLikedSalePostCommand> logger, ILikedSalePostRepository likedSalePostRepository)
        {
            _requestClient = requestClient;
            _logger = logger;
            _likedSalePostRepository = likedSalePostRepository;
            _mapper = mapper;
        }
        

        public async Task<LikedSalePostDtoResponse> Handle(DeleteLikedSalePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _likedSalePostRepository.RemoveLikedSalePosts(request.LikedSalePostDtoRequest, cancellationToken);
                _logger.LogInformation("Deleting a like for user {uid} for Sale post {Id}", request.LikedSalePostDtoRequest.UserId, request.LikedSalePostDtoRequest.SalePostId);
                //likedSalePost.SalePost = new Domain.SalePost();
                var mapped = _mapper.Map<LikedSalePostDtoResponse>(request.LikedSalePostDtoRequest);
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

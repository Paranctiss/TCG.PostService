using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.Contracts;
using TCG.PostService.Application.LikedSalePost.DTO.Response;
using TCG.PostService.Application.SalePost.Command;

namespace TCG.PostService.Application.LikedSalePost.Query
{
    public record GetLikedSalePostByUserQuery(int Id) : IRequest<IEnumerable<LikedSalePostDtoResponse>>;

    public class GetLikedSalePostByUserValidator : AbstractValidator<GetLikedSalePostByUserQuery>
    {
        public GetLikedSalePostByUserValidator()
        {
            _ = RuleFor(sp => sp.Id).NotEmpty();
        }
    }

    public class GetLikedSalePostByUserQueryHandler : IRequestHandler<GetLikedSalePostByUserQuery, IEnumerable<LikedSalePostDtoResponse>>
    {
        private readonly ILogger<GetLikedSalePostByUserQueryHandler> _logger;
        private readonly ILikedSalePostRepository _likedSalePostRepository;
        private readonly IMapper _mapper;

        public GetLikedSalePostByUserQueryHandler(ILogger<GetLikedSalePostByUserQueryHandler> logger, ILikedSalePostRepository likedSalePostRepository, IMapper mapper)
        {
            _logger = logger;
            _likedSalePostRepository = likedSalePostRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LikedSalePostDtoResponse>> Handle(GetLikedSalePostByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var likedSalePosts = await _likedSalePostRepository.GetLikedSalePostsByUserIdAsync(cancellationToken, request.Id);
                if (likedSalePosts == null)
                {
                    _logger.LogWarning("No available Sale post liked for this user found");
                    return null;
                }

                var likedSalePostsDto = _mapper.Map<List<LikedSalePostDtoResponse>>(likedSalePosts);

                foreach (LikedSalePostDtoResponse likedSalePostDtoResponse in likedSalePostsDto)
                {
                    if (_likedSalePostRepository.IsSalePostLiked(cancellationToken, 1, likedSalePostDtoResponse.SalePost.Id))
                    {
                        likedSalePostDtoResponse.SalePost.Liked = true;
                    }
                    else
                    {
                        likedSalePostDtoResponse.SalePost.Liked = false;
                    }
                }

                return likedSalePostsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving liked Sale post : {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}

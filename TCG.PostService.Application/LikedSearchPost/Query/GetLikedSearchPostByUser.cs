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
using TCG.PostService.Application.LikedSearchPost.DTO.Response;
using TCG.PostService.Application.SearchPost.Command;

namespace TCG.PostService.Application.LikedSearchPost.Query
{
    public record GetLikedSearchPostByUserQuery(int Id) : IRequest<IEnumerable<LikedSearchPostDtoResponse>>;

    public class GetLikedSearchPostByUserValidator : AbstractValidator<GetLikedSearchPostByUserQuery>
    {
        public GetLikedSearchPostByUserValidator()
        {
            _ = RuleFor(sp => sp.Id).NotEmpty();
        }
    }

    public class GetLikedSearchPostByUserQueryHandler : IRequestHandler<GetLikedSearchPostByUserQuery, IEnumerable<LikedSearchPostDtoResponse>>
    {
        private readonly ILogger<GetLikedSearchPostByUserQueryHandler> _logger;
        private readonly ILikedSearchPostRepository _likedSearchPostRepository;
        private readonly IMapper _mapper;

        public GetLikedSearchPostByUserQueryHandler(ILogger<GetLikedSearchPostByUserQueryHandler> logger, ILikedSearchPostRepository likedSearchPostRepository, IMapper mapper)
        {
            _logger = logger;
            _likedSearchPostRepository = likedSearchPostRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LikedSearchPostDtoResponse>> Handle(GetLikedSearchPostByUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var likedSearchPosts = await _likedSearchPostRepository.GetLikedSearchPostsByUserIdAsync(cancellationToken, request.Id);
                if (likedSearchPosts == null)
                {
                    _logger.LogWarning("No available search post liked for this user found");
                    return null;
                }

                var likedSearchPostsDto = _mapper.Map<List<LikedSearchPostDtoResponse>>(likedSearchPosts);

                foreach (LikedSearchPostDtoResponse likedSearchPostDtoResponse in likedSearchPostsDto)
                {
                    if (_likedSearchPostRepository.IsSearchPostLiked(cancellationToken, 1, likedSearchPostDtoResponse.SearchPost.Id))
                    {
                        likedSearchPostDtoResponse.SearchPost.Liked = true;
                    }
                    else
                    {
                        likedSearchPostDtoResponse.SearchPost.Liked = false;
                    }
                }

                return likedSearchPostsDto;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving liked search post : {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}

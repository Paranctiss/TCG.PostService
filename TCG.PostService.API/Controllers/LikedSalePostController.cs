using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCG.PostService.Application.LikedSalePost.Command;
using TCG.PostService.Application.LikedSalePost.DTO.Request;
using TCG.PostService.Application.LikedSalePost.DTO.Response;
using TCG.PostService.Application.LikedSalePost.Query;
using TCG.PostService.Application.SalePost.Command;
using TCG.PostService.Application.SalePost.Query;

namespace TCG.PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikedSalePostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LikedSalePostController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetAllLikedSalePostByUser(int id, CancellationToken cancellationToken)
        {
            var salePost = await _mediator.Send(new GetLikedSalePostByUserQuery(id), cancellationToken);

            if (salePost == null)
            {
                return NotFound();
            }
            return Ok(salePost);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLikeSalePost([FromBody] LikedSalePostDtoRequest likedSalePostDto, CancellationToken cancellationToken)
        {
            var command = new PostLikedSalePostCommand(likedSalePostDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLikedSalePost([FromBody] LikedSalePostDtoRequest likedSalePostDto, CancellationToken cancellationToken)
        {
            var command = new DeleteLikedSalePostCommand(likedSalePostDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok();
        }

    }
}

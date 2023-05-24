﻿using FluentValidation;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TCG.PostService.Application.LikedSearchPost.Command;
using TCG.PostService.Application.LikedSearchPost.DTO.Request;
using TCG.PostService.Application.LikedSearchPost.DTO.Response;
using TCG.PostService.Application.LikedSearchPost.Query;
using TCG.PostService.Application.SearchPost.Command;
using TCG.PostService.Application.SearchPost.Query;

namespace TCG.PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LikedSearchPostController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LikedSearchPostController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> GetAllLikedSearchPostByUser(int id, CancellationToken cancellationToken)
        {
            var searchPost = await _mediator.Send(new GetLikedSearchPostByUserQuery(id), cancellationToken);

            if (searchPost == null)
            {
                return NotFound();
            }
            return Ok(searchPost);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLikeSearchPost([FromBody] LikedSearchPostDtoRequest likedSearchPostDto, CancellationToken cancellationToken)
        {
            var command = new PostLikedSearchPostCommand(likedSearchPostDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok();
            //return CreatedAtAction("SearchPost Liked", new { id = result.SearchPostId }, result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLikedSearchPost([FromBody] LikedSearchPostDtoRequest likedSearchPostDto, CancellationToken cancellationToken)
        {
            var command = new DeleteLikedSearchPostCommand(likedSearchPostDto);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok();
            //return CreatedAtAction("SearchPost Liked", new { id = result.SearchPostId }, result);
        }

    }
}

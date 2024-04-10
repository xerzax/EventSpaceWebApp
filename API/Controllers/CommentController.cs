using Application.DTOs.Comment;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _commentServices;

		public CommentController(ICommentService likeServices)
		{
			_commentServices = likeServices;
		}

		[HttpPost]

		public async Task<IActionResult> CommentPost(CommentRequestDTO request)
		{
			var result = await _commentServices.CommentPostAsync(request);

			return Ok(result);
		}

		[HttpGet]

		public async Task<IActionResult> GetTotalComments(int postId, string postType)
		{
			var result = _commentServices.GetTotalLikes(postId, postType);
			return Ok(result);
		}

		[HttpPut]

		public async Task<IActionResult> EditComment(int commentId, string content)
		{
			var result = await _commentServices.EditComment(commentId, content);

			return Ok(result);
		}
	}
}

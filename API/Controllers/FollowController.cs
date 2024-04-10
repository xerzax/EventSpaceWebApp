using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class FollowController : ControllerBase
	{
		private readonly IFollowService _followService;

		public FollowController(IFollowService followService)
		{
			_followService = followService;
		}

		[HttpPost]

		public async Task<IActionResult> FollowUser(Guid UserId)
		{
			var result = await _followService.FollowUser(UserId);

			if (result == true)
			{
				return Ok("User Followed");
			}
			return Ok("Failed to follow user");
		}
	}
}

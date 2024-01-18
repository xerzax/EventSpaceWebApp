using EventSpace.Shared.Entities.Post;
using EventSpace.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventSpace.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BlogController : ControllerBase
	{
		private readonly IBlogService _blogService;

		public BlogController(IBlogService blogService)
		{
			_blogService = blogService;
		}


		[HttpGet("GetAllBlogs")]
		public async Task<IActionResult> GetAllBlogs()
		{
			try
			{
				var blogs = await _blogService.GetAllBlogAsync();
				return Ok(blogs);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("GetBlogsById")]
		public async Task<IActionResult> GetBlogsById(int id) 
		{
			try
			{
				var blog = await _blogService.GetBlogByIdAsync(id);
				if (blog == null)
				{
					return NotFound();
				}
				return Ok(blog);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}

		}

		[HttpPost("PostBlogs")]
		public async Task<ActionResult<Blog>> AddBlogs([FromBody] Blog blog)
		{
			try
			{
				var addedBlog = await _blogService.AddBlogAsync(blog);
				return CreatedAtAction(nameof(GetBlogsById), new { id = addedBlog.Id }, addedBlog);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog blog)
		{
			if (id != blog.Id)
			{
				return BadRequest();
			}

			try
			{
				await _blogService.UpdateBlogAsync(blog);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlog(int id)
		{
			try
			{
				await _blogService.DeleteBlogByIdAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}

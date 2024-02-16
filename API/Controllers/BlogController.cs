using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
	[Authorize]
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
			
				var blogs = await _blogService.GetAllBlogsAsync();
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
				var blogById = await _blogService.GetByIdBlogAsync(id);
				if(blogById == null)
				{
					return NotFound();
				}
				return Ok(blogById);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("PostBlogs")]
		public async Task<ActionResult<Blog>> AddBlogs([FromBody] BlogDTO blog)
		{
			try
			{
				//var claimsIdentity = (ClaimsIdentity)User.Identity;
				//var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			
				var addedBlog = await _blogService.AddBlogAsync(blog);
				return CreatedAtAction(nameof(GetBlogsById), new { id = addedBlog.Id }, addedBlog);
			}
			catch(Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBlogs(int id, [FromBody] Blog blog)
		{
			if(id != blog.Id)
			{
				return BadRequest();
			}
			try
			{
				await _blogService.UpdateBlogAsync(blog);
				return NoContent();
			}
			catch(Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBlogs(int id)
		{
			try
			{
				await _blogService.DeleteBlogAsync(id);
				return NoContent();
			}
			catch(Exception ex) 
			{
				return StatusCode(500, ex.Message);
			}
		}

	}
}

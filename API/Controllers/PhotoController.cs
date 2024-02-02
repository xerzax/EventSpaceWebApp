using Application.Interfaces.Services;
using Domain.Entity.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhotoController : ControllerBase
	{
		private readonly IPhotoService _photoService;

		public PhotoController(IPhotoService photoService)
		{
			_photoService = photoService;
		}

		[HttpGet("GetAllPhotos")]
		public async Task<IActionResult> GetAllPhotos()
		{
			try
			{
				var allPhotos = await _photoService.GetAllPhotosAsync();
				return Ok(allPhotos);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("GetPhotoById")]
		public async Task<IActionResult> GetPhotoById(int id)
		{
			try
			{
				var photoById = await _photoService.GetByPhotoIdAsync(id);
				if (photoById == null)
				{
					return NotFound();
				}
				return Ok(photoById);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("PostPhotos")]
		public async Task<ActionResult<Photo>> AddPhotos([FromBody] Photo photos)
		{
			try
			{
				var addedPhoto = await _photoService.AddPhotosAsync(photos);
				return CreatedAtAction(nameof(AddPhotos), new { id = addedPhoto.Id }, addedPhoto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePhoto(int id, [FromBody] Photo photo)
		{
			if (id != photo.Id)
			{
				return BadRequest();
			}
			try
			{
				await _photoService.UpdatePhotosAsync(photo);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePhotoById(int id)
		{
			try
			{
				await _photoService.DeletePhotosAsync(id);
				return NoContent();
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}


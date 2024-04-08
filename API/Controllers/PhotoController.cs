using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Infrastructure.Implementation.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using System.Reflection.Metadata;

namespace API.Controllers
{
	[Authorize]

	[Route("api/[controller]")]
	[ApiController]
	public class PhotoController : ControllerBase
	{
		private readonly IPhotoService _photoService;
		private readonly IFileService _fileService;

		public PhotoController(IPhotoService photoService, IFileService fileService)
		{
			_photoService = photoService;
			_fileService = fileService;
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

		/*[HttpPost("PostPhotos")]
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
*/
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
				await _fileService.DeleteUserUpload(id);
				return Ok("Photo Deleted!");
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("insertPic")]
		public async Task<IActionResult> InsertPhoto(PhotoDTO photo)
		{
			try
			{
				await _photoService.InsertPhoto(photo);
				var result = "Photo added!";
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		/*[HttpPost("addPhoto")]
		public async Task<IActionResult> PostPhoto(Photo photo)
		{ 
			var addPhoto = await _photoService.AddPhotosAsync(photo);
			return Ok(addPhoto);
		}*/

	}
}


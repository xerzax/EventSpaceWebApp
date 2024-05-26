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

		[Authorize]

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
		[Authorize]

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
		[Authorize]


		[HttpPost("insertPic")]
		public async Task<ActionResult<Photo>> InsertPhoto(PhotoDTO photo)
		{
			try
			{
				var addedPhoto = await _photoService.InsertPhoto(photo);
				return CreatedAtAction(nameof(GetPhotoById), new { id = addedPhoto.Id }, addedPhoto);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}


using Application.DTOs;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlaylistController : ControllerBase
	{
		private readonly IPlaylistService _playlistService;

		public PlaylistController(IPlaylistService playlistService )
		{
			_playlistService = playlistService;
		}

		[HttpGet("GetAllPlaylist")]
		public async Task<IActionResult> GetAllPlaylist()
		{
			var playlists = await _playlistService.GetAllPlaylistAsync();
			return Ok(playlists);
		}

		[HttpGet("GetPlaylistById")]
		public async Task<IActionResult> GetPlaylistById(int id)
		{
			try
			{
				var playlistbById = await _playlistService.GetPlaylistByIdAsync(id);
				if (playlistbById == null)
				{
					return NotFound();
				}
				return Ok(playlistbById);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpPost("AddPlaylist")]
		public async Task<ActionResult<PlaylistDTO>> AddPlaylist([FromBody]PlaylistDTO playlistDTO)
		{
			if(playlistDTO == null)
			{
				return BadRequest();
			}

			var playlist = new Playlist
			{
				Title = playlistDTO.Title,
				UserId = playlistDTO.UserId,
				UserName = playlistDTO.UserName,
			};
			await _playlistService.AddPlaylistAsync(playlist);
			return CreatedAtAction(nameof(AddPlaylist),new {id = playlist.Id} ,playlistDTO);
		}
		[HttpDelete("{id}")]
		public async Task<ActionResult> DeletePlaylist(int id)
		{
			await  _playlistService.DeletePlaylistAsync(id);
			return NoContent();
		}

		[HttpPut("{id}")]
		public async Task<ActionResult> UpdatePlaylist(int id, [FromBody]Playlist playlist )
		{
			if( playlist == null || id != playlist.Id)
			{
				return BadRequest();
			}
			
			var existingPlaylist = await _playlistService.GetPlaylistByIdAsync(playlist.Id);
			if(existingPlaylist == null)
			{
				return BadRequest();
			}

			existingPlaylist.Title = playlist.Title; 
			existingPlaylist.UserId = playlist.UserId;
			existingPlaylist.UserName = playlist.UserName;

			await _playlistService.UpdatePlaylistAsync(playlist);
			return NoContent();
		}
	}
}

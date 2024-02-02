using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;

namespace Infrastructure.Implementation.Services
{
	public class PlaylistService : IPlaylistService
	{
		private readonly IGenericRepository<Playlist> _playlistRepository;

		public PlaylistService(IGenericRepository<Playlist> playlistRepository)
		{
			_playlistRepository = playlistRepository ?? throw new ArgumentNullException(nameof(playlistRepository));
		}

		public async Task<IEnumerable<PlaylistDTO>> GetAllPlaylistAsync()
		{
			var playlists = await _playlistRepository.GetAllAsync();
			return MapToPlaylistDTOs(playlists);
		}

		public async Task<PlaylistDTO> GetPlaylistByIdAsync(int id)
		{
			var playlist = await _playlistRepository.GetByIdAsync(id);
			return MapToPlaylistDTO(playlist);
		}

		public async Task<Playlist> AddPlaylistAsync(Playlist playlist)
		{
			if (playlist == null)
			{
				throw new ArgumentNullException(nameof(playlist));
			}

			await _playlistRepository.AddAsync(playlist);
			return playlist;
		}

		public async Task DeletePlaylistAsync(int id)
		{
			var playlist = await _playlistRepository.GetByIdAsync(id);

			if (playlist != null)
			{
				await _playlistRepository.DeleteAsync(playlist);
			}
		}

		public async Task UpdatePlaylistAsync(Playlist playlist)
		{
			if (playlist == null)
			{
				throw new ArgumentNullException(nameof(playlist));
			}

			await _playlistRepository.UpdateAsync(playlist);
		}

		private PlaylistDTO MapToPlaylistDTO(Playlist playlist)
		{
			if (playlist == null)
			{
				return null;
			}

			return new PlaylistDTO
			{
				Id = playlist.Id,
				Title = playlist.Title,
				UserId = playlist.UserId,
				UserName = playlist.UserName,
				Songs = (List<SongDTO>)MapToSongDTOs(playlist.Songs)
			};
		}

		private IEnumerable<PlaylistDTO> MapToPlaylistDTOs(IEnumerable<Playlist> playlists)
		{
			var playlistDtos = new List<PlaylistDTO>();

			foreach (var playlist in playlists)
			{
				var playlistDto = MapToPlaylistDTO(playlist);
				playlistDtos.Add(playlistDto);
			}

			return playlistDtos;
		}

		private IEnumerable<SongDTO> MapToSongDTOs(IEnumerable<Song> songs)
		{
			var songDtos = new List<SongDTO>();

			foreach (var song in songs)
			{
				var songDto = new SongDTO
				{
					VideoCode = song.VideoCode,
					Title = song.Title
				};

				songDtos.Add(songDto);
			}
			return songDtos;
		}
	}
}

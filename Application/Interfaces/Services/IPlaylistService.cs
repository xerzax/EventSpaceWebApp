using Application.DTOs;
using Application.DTOs.Playlist;
using Application.DTOs.Post;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface IPlaylistService
	{
        Task<IEnumerable<PlaylistDTO>> GetAllPlaylistAsync();
        Task<PlaylistDTO> GetPlaylistByIdAsync(int id);
        Task<Playlist> AddPlaylistAsync(Playlist playlists);
        Task DeletePlaylistAsync(int id);
        Task UpdatePlaylistAsync(Playlist playlist);

        Task<string> CreatePlaylist(string Title, List<SongDRequestTO> selectedSongs);

        Task<List<PlaylistResponseDTO>> GetAllPlaylist();

        Task<PlaylistByIdResponseDTO> GetPlaylistById(int id);
    }
}

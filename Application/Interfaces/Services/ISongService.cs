using Application.DTOs;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
	public interface ISongService
	{
		Task<IEnumerable<SongDTO>> GetAllSongsAsync();
		Task<SongDTO> GetSongAsync(int id);
		Task<Song> AddSongAsync(Song song);
		Task DeleteSongAsync(int id);
		Task UpdateSongAsync(Song song);
	}
}

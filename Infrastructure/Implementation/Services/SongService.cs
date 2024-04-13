using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
    public class SongService : ISongService
    {
        private readonly IGenericRepository<Song> _songRepository;

        public SongService(IGenericRepository<Song> songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<Song> AddSongAsync(Song song)
        {
            var addedSong = await _songRepository.AddAsync(song);
            return addedSong;
        }

        public async Task DeleteSongAsync(int id)
        {
            var songById = await _songRepository.GetByIdAsync(id);
            if (songById != null)
            {
                await _songRepository.DeleteAsync(songById);
            }
        }

        public async Task<IEnumerable<SongDTO>> GetAllSongsAsync()
        {
            var songs = await _songRepository.GetAllAsync();
            var result = new List<SongDTO>();
            foreach (var song in songs)
            {
                result.Add(new SongDTO
                {
                    Id = song.Id,
                    Title = song.Title,
                    VideoCode = song.VideoCode,
                    PlaylistId = song.PlaylistId,
                });
            }
            return result.ToList();
        }

        public async Task<SongDTO> GetSongByIdAsync(int id)
        {
            var songById = await _songRepository.GetByIdAsync(id);
            var result = new SongDTO
            {
                Id = songById.Id,
                Title = songById.Title,
            };
            return result;
        }

        public async Task<List<SongDTO>> GetSongByIdPlaylistAsync(int id)
        {
            List<SongDTO> songsList = new List<SongDTO>();
            var  songs = await _songRepository.Where(x => x.PlaylistId == id);
            foreach(var song in songs)
            {
                songsList.Add(new SongDTO
                {
                    Id = song.Id,
                    Title = song.Title,
                    VideoCode = song.VideoCode,
                    PlaylistId = song.PlaylistId,
                });

            }

            return songsList;




        }

        public async Task UpdateSongAsync(Song song)
        {
            var songToUpdate = await _songRepository.GetByIdAsync(song.Id);
            if (songToUpdate != null)
            {
                songToUpdate.Title = song.Title;
                await _songRepository.UpdateAsync(songToUpdate);
            }
        }
    }
}

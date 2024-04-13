using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Playlist;
using Application.DTOs.Post;
using Application.DTOs.Song;
using Application.Interfaces.Identity;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using Domain.Entity.Post;

namespace Infrastructure.Implementation.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IGenericRepository<Playlist> _playlistRepository;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IGenericRepository<Song> _songRepository;

        public PlaylistService(IGenericRepository<Playlist> playlistRepository, IUserIdentityService userIdentityService, IGenericRepository<Song> songRepository)
        {
            _playlistRepository = playlistRepository;
            _userIdentityService = userIdentityService;
            _songRepository = songRepository;
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
                //Songs = (List<SongDTO>)MapToSongDTOs(playlist.Songs)
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

        public async Task<string> CreatePlaylist(string Title, List<SongDRequestTO> selectedSongs)
        {
            var user = _userIdentityService.GetLoggedInUser();
            var playlist = new Playlist
            {
                Title = Title,
                CreatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
                IsDeleted = false,
                LastUpdatedAt = DateTime.Now,
                UserId = user.UserId,
                UserName = user.UserName,
            };
            await _playlistRepository.AddAsync(playlist);

            foreach (var songs in selectedSongs)
            {
                var song = new Song
                {
                    Title = songs.Title,
                    PlaylistId = playlist.Id,
                    VideoCode = songs.VideoId
                };
                await _songRepository.AddAsync(song);
            }
            return "Playlist Created Successfully";
        }

        public async Task<List<PlaylistResponseDTO>> GetAllPlaylist()
        {
            var user = _userIdentityService.GetLoggedInUser();

            var playlists = await _playlistRepository.GetAllAsync();

            var loggedInUserPlaylist = playlists.Where(x => x.UserId == user.UserId);

            var result = loggedInUserPlaylist.Select(x => new PlaylistResponseDTO
            {
                Title = x.Title,
            }).ToList();

            return result;
        }

        public async Task<PlaylistByIdResponseDTO> GetPlaylistById(int id)
        {
            var playlist = await _playlistRepository.GetByIdAsync(id);

            var songs = await _songRepository.GetAllAsync();

            var playlistSongs = songs.Where(x => x.PlaylistId == playlist.Id)
                             .Select(song => new SongResponseDTO
                             {
                                 Name = song.Title,
                                 VideoCode = song.VideoCode
                             })
                             .ToList();

            var result = new PlaylistByIdResponseDTO
            {
                Title = playlist.Title,
                Songs = playlistSongs
            };
            return result;
        }
    }
}

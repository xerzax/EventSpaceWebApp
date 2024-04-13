using Application.DTOs.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Playlist
{
    public class PlaylistWithSongDTO
    {
        public int PlaylistId { get; set; }
        public string CreatedBy { get; set; }


        public string Title { get; set; }

        public List<SongDTO> Songs { get; set; }
    }
}

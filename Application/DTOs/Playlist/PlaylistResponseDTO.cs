using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Playlist
{
    public class PlaylistResponseDTO
    {

        public List<PlaylistDTO> Playlist { get; set; }

        public string Title { get; set; }

    }
}

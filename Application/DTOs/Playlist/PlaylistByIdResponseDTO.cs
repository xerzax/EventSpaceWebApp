﻿using Application.DTOs.Song;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Playlist
{
    public class PlaylistByIdResponseDTO
    {
        public string Title { get; set; }

        public List<SongResponseDTO> Songs { get; set; }
    }
}

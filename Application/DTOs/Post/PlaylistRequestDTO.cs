using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Post
{
    public class PlaylistRequestDTO
    {
            public List<SongDRequestTO> VideoList { get; set; }
            public string Title { get; set; }
    }

    public class SongDRequestTO
    {
        public string VideoId { get; set; }

        public string Title { get; set; }
    }
}

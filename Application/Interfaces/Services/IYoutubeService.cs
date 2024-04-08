using Domain.Entity.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IYoutubeService
    {
        Task<List<YoutubeSearchResult>> SearchVideosAsync(string query);
    }
}

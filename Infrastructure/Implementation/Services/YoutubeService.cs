using Application.Interfaces.Services;
using Domain.Entity.Post;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Implementation.Services
{
    public class YouTubeService : IYoutubeService
    {
        public async Task<List<YoutubeSearchResult>> SearchVideosAsync(string query)
        {
            var apiKey = "AIzaSyB-1fNmftyS9dbIQmVQh795noxk4siGiJk";

            using (var client = new HttpClient())
            {
                var json = await client.GetStringAsync(
                    $"https://www.googleapis.com/youtube/v3/search?part=snippet&q={query}&maxResults=10&type=video&key={apiKey}");

                var results = JsonConvert.DeserializeObject<dynamic>(json);
                var videos = results.items;


                var searchResults = new List<YoutubeSearchResult>();
                foreach (var video in videos)
                {
                    searchResults.Add(new YoutubeSearchResult
                    {
                        Title = video.snippet.title,
                        Description = video.snippet.description,
                        ThumbnailUrl = video.snippet.thumbnails.@default.url,
                        VideoId = video.id.videoId,
                        ChannelTitle = video.snippet.channelTitle
                    });
                }

                return searchResults;
            }
        }
    }
}

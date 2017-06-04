using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Playlist : CommandCollection
    {
        private readonly Request _request;

        internal Playlist(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Add item(s) to playlist.</summary>
        public async Task AddAsync(int playlistId, int? songId = null, int? albumId = null, int? movieId = null, int? episodeId = null)
        {
            var method = new ParameteredMethodMessage<PlaylistAdd>
            {
                Method = "Playlist.Add",
                Parameters = new PlaylistAdd
                {
                    PlaylistId = playlistId,
                    Item = new PlaylistItem { SongId = songId, AlbumId = albumId, MovieId = movieId, EpisodeId = episodeId }
                }
            };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }

        /// <summary>Clear playlist.</summary>
        public async Task ClearAsync(int playlistId)
        {
            var method = new ParameteredMethodMessage<PlaylistIdParameters>
                             {
                                 Method = "Playlist.Clear",
                                 Parameters = new PlaylistIdParameters {PlaylistId = playlistId}
                             };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }

        /// <summary>Get all items from playlist.</summary>
        public async Task<PlaylistGetItems> GetItemsAsync(int playlistId, params ListFieldsAll[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(ListFieldsAll));

            var method = new ParameteredMethodMessage<PlaylistParameters>
            {
                Method = "Playlist.GetItems",
                Parameters = new PlaylistParameters
                {
                    PlayListId = playlistId,
                    Properties = properties
                }
            };

            var item = await _request.SendRequestAsync<BasicResponseMessage<PlaylistGetItems>>(method);
            return item.Result;
        }

        /// <summary>Returns all existing playlists.</summary>
        public async Task<Model.Playlist[]> GetPlaylistsAsync()
        {
            var result = await _request.SendRequestAsync<BasicResponseMessage<Model.Playlist[]>>("Playlist.GetPlaylists");
            return result.Result;
        }

        public async Task<string> RemoveAsync(int playlistId, int position)
        {
            var method = new ParameteredMethodMessage<PlaylistRemove>
            {
                Method = "Playlist.Remove",
                Parameters = new PlaylistRemove { PlaylistId = playlistId, Position = position }
            };

            var item = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return item.Result;
        }
    }
}
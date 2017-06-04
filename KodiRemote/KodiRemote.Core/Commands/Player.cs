using System;
using System.Linq;
using System.Threading.Tasks;
using KodiRemote.Core.Model;
using KodiRemote.Core.Requests;
using KodiRemote.Core.Responses;

namespace KodiRemote.Core.Commands
{
    public sealed class Player : CommandCollection
    {
        private readonly Request _request;

        internal Player(Connection xbmc)
        {
            _request = new Request(xbmc);
        }

        /// <summary>Returns all active players.</summary>
        public async Task<Model.Player[]> GetActivePlayersAsync()
        {
            var result = await _request.SendRequestAsync<BasicResponseMessage<Model.Player[]>>("Player.GetActivePlayers");
            return result.Result;
        }

        /// <summary>Retrieves the currently played item.</summary>
        public async Task<ListItemAll> GetItemAsync(int playerId, params ListFieldsAll[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(ListFieldsAll));

            var method = new ParameteredMethodMessage<PlayerGetItemParameters>
                             {
                                 Method = "Player.GetItem",
                                 Parameters = new PlayerGetItemParameters
                                                  {
                                                      PlayerId = playerId,
                                                      Properties = properties
                                                  }
                             };

            var item = await _request.SendRequestAsync<SingleValueResponseMessage<GetItemResponse>>(method);
            return item.Result.Item;
        }

        /// <summary>Retrieves the values of the given properties.</summary>
        public async Task<PlayerPropertyValue> GetPropertiesAsync(int playerId, params PlayerPropertyName[] fields)
        {
            string[] properties = fields.Select(p => p.ToString().ToLowerInvariant()).ToArray();

            if (!properties.Any())
                properties = Enum.GetNames(typeof(PlayerPropertyName));

            var method = new ParameteredMethodMessage<PlayerGetItemParameters>
                             {
                                 Method = "Player.GetProperties",
                                 Parameters = new PlayerGetItemParameters
                                                  {
                                                      PlayerId = playerId,
                                                      Properties = properties
                                                  }
                             };

            var item = await _request.SendRequestAsync<SingleValueResponseMessage<PlayerPropertyValue>>(method);
            return item.Result;
        }

        /// <summary>Go to previous/next/specific item in the playlist.</summary>
        public async Task<string> GoToAsync(int playerId, GoTos where)
        {
            var method = new ParameteredMethodMessage<GoToParameters>
                             {
                                 Method = "Player.GoTo",
                                 Parameters = new GoToParameters
                                                  {
                                                      PlayerId = playerId,
                                                      To = where.ToString().ToLowerInvariant()
                                                  }
                             };

            var result = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return result.Result;
        }

        /// <summary>Start playback of either the playlist with the given ID, a slideshow with the pictures from the given directory or a single file or an item from the database.</summary>
        public async Task<string> OpenAsync(int? songId = null, int? albumId = null, int? movieId = null, int? episodeId = null)
        {
            var method = new ParameteredMethodMessage<OpenParameters<PlaylistItem>>
                {
                    Method = "Player.Open",
                    Parameters = new OpenParameters<PlaylistItem>
                        {
                            Item = new PlaylistItem { SongId = songId, AlbumId = albumId, MovieId = movieId, EpisodeId = episodeId },
                            Options = new OpenOptions {  Repeat = PlayerRepeat.Off.ToString().ToLowerInvariant() }
                        }
                };

            var item = await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
            return item.Result;
        }

        /// <summary>Pauses or unpause playback and returns the new state.</summary>
        public async Task PlayPauseAsync(int playerId)
        {
            var method = new ParameteredMethodMessage<PlayerParameters>
                             {
                                 Method = "Player.PlayPause",
                                 Parameters = new PlayerParameters { PlayerId = playerId }
                             };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }

        /// <summary>Seek through the playing item.</summary>
        public async Task SeekAsync(int playerId, double percentage)
        {
            var parameters = new PlayerPositionPercentageParameters
                                 {
                                     PlayerId = playerId,
                                     Value = percentage
                                 };

            await SeekAsync(parameters);
        }

        /// <summary>Seek through the playing item.</summary>
        public async Task SeekAsync(int playerId, int milliseconds = 0, int seconds = 0, int minutes = 0, int hours = 0)
        {
            var parameters = new PlayerPositionTimeParameters
                                 {
                                     PlayerId = playerId,
                                     Value = new GlobalTime
                                                 {
                                                     Hours = hours,
                                                     Minutes = minutes,
                                                     Seconds = seconds,
                                                     Milliseconds = milliseconds
                                                 }
                                 };

            await SeekAsync(parameters);
        }

        /// <summary>Seek through the playing item.</summary>
        public async Task SeekAsync(int playerId, Seeks seek)
        {
            var parameters = new PlayerPositionEnumParameters
                                 {
                                     PlayerId = playerId,
                                     Value = seek.ToString().ToLowerInvariant()
                                 };

            await SeekAsync(parameters);
        }

        /// <summary>Seek through the playing item.</summary>
        private async Task SeekAsync<T>(T parameters)
            where T: PlayerPositionParameters
        {
            var method = new ParameteredMethodMessage<T>
                             {
                                 Method = "Player.Seek",
                                 Parameters = parameters
                             };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }

        /// <summary>Stops playback.</summary>
        public async Task StopAsync(int playerId)
        {
            var method = new ParameteredMethodMessage<PlayerParameters>
                             {
                                 Method = "Player.Stop",
                                 Parameters = new PlayerParameters { PlayerId = playerId }
                             };

            await _request.SendRequestAsync<BasicResponseMessage<string>>(method);
        }
    }
}